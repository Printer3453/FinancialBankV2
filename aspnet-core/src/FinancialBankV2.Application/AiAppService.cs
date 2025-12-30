using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using FinancialBankV2.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Text; // StringBuilder için gerekli

namespace FinancialBankV2
{
    [Authorize(FinancialBankV2Permissions.Ai.Default)]
    public class AiAppService : ApplicationService, IAiAppService
    {
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly IRepository<Transaction, Guid> _transactionRepository;
        private readonly ICurrentUser _currentUser;

        // Ayarlar
        private const string OllamaApiUrl = "http://localhost:11434/v1";
        private const string AiModelName = "llama3.2:latest";

        public AiAppService(
            IRepository<BankAccount, Guid> bankAccountRepository,
            IRepository<Transaction, Guid> transactionRepository,
            ICurrentUser currentUser)
        {
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _currentUser = currentUser;
        }

        // ==================================================================================
        // 1. ANA METOT (YÖNETİCİ KATMANI)
        // Karmaşık mantık yok, sadece alt ekipleri yönetir.
        // ==================================================================================
        public async Task<AiAnswerDto> AskQuestionAsync(AskQuestionDto input)
        {
            if (!_currentUser.IsAuthenticated)
                throw new UserFriendlyException("AI Asistanı kullanmak için lütfen giriş yapın.");

            // 1. Veriyi Getir
            var userAccounts = await _bankAccountRepository.GetListAsync(a => a.UserId == _currentUser.Id.Value);

            // 2. Karar Ver (Kullanıcı ne soruyor? Hangi veriyi vermeliyim?)
            var contextData = await BuildContextAndInstructionAsync(input.Question, userAccounts);

            // 3. İşi Yap (AI'ya gönder)
            var aiResponse = await GetResponseAsync(contextData.UserContext, contextData.SystemInstruction, input.Question);

            return new AiAnswerDto { Answer = aiResponse };
        }

        // ==================================================================================
        // 2. CONTEXT BUILDER (KARAR MEKANİZMASI)
        // Soruyu analiz eder, veriyi hazırlar ve AI'ya ne yapacağını söyler.
        // ==================================================================================
        private async Task<(string UserContext, string SystemInstruction)> BuildContextAndInstructionAsync(string question, List<BankAccount> accounts)
        {
            // Kullanıcının hiç hesabı yoksa
            if (!accounts.Any())
            {
                return (
                    UserContext: "Kullanıcının henüz bir banka hesabı yok.", 
                    SystemInstruction: "Sen yardımsever bir asistansın. Kullanıcıya hesap açması için yol göster."
                );
            }

            var q = question.ToLower();

            // SENARYO A: Bakiye Sorusu
            if (q.Contains("bakiye") || q.Contains("param") || q.Contains("kadar"))
            {
                var totalBalance = accounts.Sum(x => x.Balance);
                return (
                    UserContext: $"Kullanıcının Toplam Bakiyesi: {totalBalance:N2} TL",
                    SystemInstruction: "Sen bir banka asistanısın. Kullanıcıya sadece bakiyesini söyle. Kısa, net ve resmi ol."
                );
            }

            // SENARYO B: Hesap Hareketleri
            if (q.Contains("hareket") || q.Contains("geçmiş") || q.Contains("harcama") || q.Contains("transfer"))
            {
                var transactionHistory = await GetTransactionHistoryTextAsync(accounts);
                return (
                    UserContext: transactionHistory,
                    SystemInstruction: "Sen bir banka asistanısın. Sana verilen işlem listesini kullanıcıya sun. Her işlemi mutlaka yeni bir satıra yaz. Okunabilir olsun."
                );
            }

            // SENARYO C: Genel Sohbet (Merhaba, Nasılsın vb.)
            return (
                UserContext: $"Kullanıcı giriş yapmış durumda. Toplam {accounts.Count} adet hesabı var.",
                SystemInstruction: "Sen FinancialBankV2'nin yapay zeka asistanısın. Kullanıcıyla nazikçe sohbet et ve bankacılık işlemlerinde yardımcı olabileceğini belirt."
            );
        }

        // ==================================================================================
        // 3. VERİ FORMATLAMA (AMELELİK KATMANI)
        // Veritabanından gelen listeyi güzel bir string metne çevirir.
        // ==================================================================================
        private async Task<string> GetTransactionHistoryTextAsync(List<BankAccount> accounts)
        {
            var accountIds = accounts.Select(x => x.Id).ToList();
            
            var transactions = await _transactionRepository.GetListAsync(t =>
                accountIds.Contains(t.SenderAccountId) || accountIds.Contains(t.ReceiverAccountId));

            var lastTransactions = transactions.OrderByDescending(t => t.TransactionDate).Take(5).ToList();

            if (!lastTransactions.Any()) 
                return "Hiç işlem bulunamadı.";

            var sb = new StringBuilder("Son 5 İşlem Listesi:\n");
            
            foreach (var tx in lastTransactions)
            {
                bool isExpense = accountIds.Contains(tx.SenderAccountId);
                string symbol = isExpense ? "(-)" : "(+)";
                sb.AppendLine($"{symbol} {tx.Amount:N2} TL | {tx.TransactionDate:dd.MM HH:mm}");
            }

            return sb.ToString();
        }

        // ==================================================================================
        // 4. AI ENGINE (TEKNİK KATMAN)
        // Sadece Semantic Kernel ve Ollama bağlantısını yönetir.
        // ==================================================================================
        private async Task<string> GetResponseAsync(string userContext, string systemInstruction, string userQuestion)
        {
            try
            {
                var builder = Kernel.CreateBuilder();
                builder.AddOpenAIChatCompletion(modelId: AiModelName, apiKey: "Ollama", endpoint: new Uri(OllamaApiUrl));
                
                var kernel = builder.Build();
                var chatService = kernel.GetRequiredService<IChatCompletionService>();
                
                var history = new ChatHistory();
                history.AddSystemMessage($@"
                    You are a helpful AI Assistant.
                    RULES: Answer in clear TURKISH. Do not hallucinate. Be concise.
                    INSTRUCTIONS: {systemInstruction}
                    DATA: {userContext}");
                
                history.AddUserMessage(userQuestion);

                var response = await chatService.GetChatMessageContentAsync(
                    history, 
                    new OpenAIPromptExecutionSettings { MaxTokens = 250, Temperature = 0.1 }, 
                    kernel: kernel
                );

                return response.Content ?? "Bir yanıt oluşturamadım.";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"AI Servisi şu an yanıt veremiyor. (Hata: {ex.Message})");
            }
        }
    }
}