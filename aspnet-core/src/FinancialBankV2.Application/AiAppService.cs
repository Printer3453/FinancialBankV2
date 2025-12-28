using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Config okumak için
using Microsoft.SemanticKernel; // Semantic Kernel kütüphanesi
using Microsoft.SemanticKernel.ChatCompletion; // Chat özellikleri
using Microsoft.SemanticKernel.Connectors.OpenAI; // OpenAI/Ollama bağlantısı
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using FinancialBankV2.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;



namespace FinancialBankV2
{
    [Authorize(FinancialBankV2Permissions.Ai.Default)]
    public class AiAppService : ApplicationService, IAiAppService
    {
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly ICurrentUser _currentUser;


        // Ollama nın çalıştığı yerel adres (Localhost)
        private const string OllamaApiUrl = "http://localhost:11434/v1";
        //Kullanacağımız model 
        private const string AiModelName = "llama3.2:latest";

        public AiAppService(
            IRepository<BankAccount, Guid> bankAccountRepostory,
            ICurrentUser currentUser
            )
        {
            _bankAccountRepository = bankAccountRepostory;
            _currentUser = currentUser;
        }

        public async Task<AiAnswerDto> AskQuestionAsync(AskQuestionDto input)
        {
            // 1. Validations (Kontroller)
            if (!_currentUser.IsAuthenticated)
            {
                throw new UserFriendlyException("Please log in to use the AI Assistant.");
            }

            // 2. RAG Context Building (Veri Toplama)
            // Kullanıcı verisini veritabanından çekip hazırlıyoruz.
            string contextData = "Kullanıcı hakkında bilgi bulunamadı.";

            var userAccounts = await _bankAccountRepository.GetListAsync(a => a.UserId == _currentUser.Id);

            if (userAccounts.Any())
            {

                var totalBalance = userAccounts.Sum(x => x.Balance);
                var accountCount = userAccounts.Count;

                contextData = $"User has {accountCount} bank accounts. Total Balance across all accounts: {totalBalance:N2} TL.";

            }


            // 3. Semantic Kernel Builder (Microsoft Standartları)
            // HTTP Client yerine Kernel oluşturuyoruz.
            var builder = Kernel.CreateBuilder();

            // Ollama aslında OpenAI uyumlu bir API sunar. Semantic Kernel'in OpenAI konektörünü kullanacağız.
            // ÖNEMLİ: Ollama için API Key gerekmez ama parametre boş geçilemez, rastgele bir şey yazıyoruz.

            builder.AddOpenAIChatCompletion(
              modelId: AiModelName,
              apiKey: "Ollama",
              endpoint: new Uri(OllamaApiUrl)
            );
            var kernel = builder.Build();
            var ChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            // 4. Prompt Engineering (Sistem Ayarları)
            // AI'ya karakterini ve kurallarını burada veriyoruz.
            var history = new ChatHistory();


            // System Prompt: Yapay zekanın "kişiliği" ve kuralları burada belirlenir.
            history.AddSystemMessage($@"
                You are a helpful and professional AI Assistant for 'FinancialBankV2'.
                Your goal is to answer user questions about their bank accounts based ONLY on the provided context.
                
                RULES:
                1. Always answer in clear, grammatically correct TURKISH language.
                2. Do not hallucinate. If you don't know, say 'Bilgim yok'.
                3. Be concise and polite.
                4. Never mention 'Parsel' or unrelated words.
                
                CONTEXT:
                {contextData}
            ");
            // User Message: Kullanıcının sorusu
            history.AddUserMessage(input.Question);

            // 5. AI Response Generation (Cevap Üretme)
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = 100,// Kısa cevap istiyoruz.

                Temperature = 0.1,// 0'a yakın olması "yaratıcılığı" öldürür, sadece gerçeği söyletir.

            };

            try
            {
                var response = await ChatCompletionService.GetChatMessageContentAsync(
                    history,
                    executionSettings,
                    kernel: kernel
                );

                return new AiAnswerDto
                {
                    Answer = response.Content ?? "I couldn't generate a response."
                };

            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi veriyoruz.
                throw new UserFriendlyException($"AI Service Error: {ex.Message}");
            }

        }

    }
}