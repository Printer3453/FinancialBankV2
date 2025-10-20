using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using FinancialBankV2.Permissions;
using Microsoft.AspNetCore.Authorization;


namespace FinancialBankV2
{
    [Authorize(FinancialBankV2Permissions.Ai.Default)]
    public class AiAppService : ApplicationService, IAiAppService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<BankAccount, Guid> _bankAccountRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWorkManager _unitOfWorkManager; // <-- YENİ
        //IUnitOfWorkManager ne yapar?
        //IUnitOfWorkManager, birim çalışması (Unit of Work) desenini yönetmek için kullanılan bir hizmettir.
        //Birim çalışması deseni, bir işlem (transaction) kapsamında birden fazla veri erişim operasyonunu tek bir birim olarak ele almayı sağlar.
        //Bu sayede, bir işlem sırasında yapılan tüm veri değişiklikleri ya tamamen başarılı olur ya da tamamen 
        // geri alınır, bu da veri tutarlılığını sağlar.   


        public AiAppService(
            IConfiguration configuration,
            IRepository<BankAccount, Guid> bankAccountRepository,
            ICurrentUser currentUser,
            IUnitOfWorkManager unitOfWorkManager) // <-- YENİ
        {
            _configuration = configuration;
            _bankAccountRepository = bankAccountRepository;
            _currentUser = currentUser;
            _unitOfWorkManager = unitOfWorkManager; // <-- YENİ
        }

        public async Task<AiAnswerDto> AskQuestionAsync(AskQuestionDto input)
        {
            // Metodun tamamını manuel bir Unit of Work içine alıyoruz
            using (var uow = _unitOfWorkManager.Begin())
            {
                if (!_currentUser.IsAuthenticated)
                {
                    throw new UserFriendlyException("Bu soruyu cevaplamak için lütfen giriş yapın.");
                }

                var apiKey = _configuration["OpenAI:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new UserFriendlyException("Yapay zeka servisi şu an aktif değil.");
                }

                string context = "";
                if (input.Question.ToLower().Contains("bakiye") || input.Question.ToLower().Contains("param"))
                {
                    var userAccounts = await _bankAccountRepository.GetListAsync(x => x.UserId == _currentUser.Id.Value);
                    var totalBalance = userAccounts.Any() ? userAccounts.Sum(x => x.Balance) : 0;
                    context = $"Kullanıcının banka hesaplarındaki toplam bakiye {totalBalance:N2} Türk Lirası.";
                }
                else
                {
                    return new AiAnswerDto { Answer = "Üzgünüm, şimdilik sadece bakiye ile ilgili soruları anlayabiliyorum." };
                }

                var openAiService = new OpenAIService(new OpenAiOptions() { ApiKey = apiKey });
                var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
                {
                    Messages = new[]
                    {
                        ChatMessage.FromSystem("Sen bir banka asistanısın. Sana verilen bilgiyi kullanarak, kullanıcının sorusuna samimi ve sadece 1 cümlelik bir cevap ver."),
                        ChatMessage.FromSystem($"Kullanılacak Bilgi: {context}"),
                        ChatMessage.FromUser(input.Question),
                    },
                    Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
                    MaxTokens = 100
                });

                if (completionResult.Successful)
                {
                    return new AiAnswerDto { Answer = completionResult.Choices[0].Message.Content };
                }
                else
                {
                    // OpenAI'den gelen asıl hata mesajını yakala
                    string errorMessage = "Bilinmeyen bir hata oluştu.";
                    if (completionResult.Error != null)
                    {
                        errorMessage = completionResult.Error.Message; // Örn: "Incorrect API key provided"
                    }
                    throw new UserFriendlyException($"Yapay zeka servisinden bir cevap alınamadı: {errorMessage}");
                }
            }
        }
    }
}