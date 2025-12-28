using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IRepository<BankAccount,Guid> _bankAccountRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        // Ollama nın çalıştığı yerel adres (Localhost)
        private const string OllamaApiUrl = "http://localhost:11434/api/generate";
        //Kullanacağımız model 
        private const string AiModelName = "llama3.2:latest";

        public AiAppService(
            IRepository<BankAccount, Guid> bankAccountRepostory,
            ICurrentUser currentUser,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _bankAccountRepository = bankAccountRepostory;
            _currentUser = currentUser;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<AiAnswerDto> AskQuestionAsync(AskQuestionDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                if(!_currentUser.IsAuthenticated)
                {
                    throw new UserFriendlyException("Bu soruyu cevaplamak için lütfen giriş yapın.");
                }
                //1. RAG (Retrieval-Augmented Generation) Kısmı:
                // Kullanıcının verisini veritabanından çekip bağlam (context) oluşturuyoruz.
                string contextData = "Kullanıcı hakkında bilgi bulunamadı.";
                if(input.Question.ToLower().Contains("bakiye") || 
                input.Question.ToLower().Contains("param") || 
                input.Question.ToLower().Contains("hesap"))
                {
                    var userAccounts = await _bankAccountRepository.GetListAsync(a => a.UserId == _currentUser.Id);
                    var totalBalance = userAccounts.Any() ? userAccounts.Sum(x => x.Balance) :0;
                    var accountCount = userAccounts.Count;

                    contextData = $"Mevcut Bilgiler: Kullanıcının toplam {accountCount} adet bankası hesabı var ve toplam bakiyesi {totalBalance} TL.";

                }
                
                var fullPrompt= $@"
                Sistem: Sen FinancialBankV2 adında güvenli bir bankanın yardımsever asistanısın. Türkçe cevap ver.
                    Bağlam (Context): {contextData}
                    Kullanıcı Sorusu: {input.Question}
                    Görevin: Yukarıdaki bağlam bilgisini kullanarak kullanıcının sorusunu kısaca cevapla. Bilgi yoksa 'Bilmiyorum' de.
                ";

                // 3. Ollama'ya İstek Gönderme
                try 
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(60); 

                        var requestBody = new
                        {
                            model = AiModelName,
                            prompt = fullPrompt,
                            stream = false 
                        };

                        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                        
                        var response = await client.PostAsync(OllamaApiUrl, jsonContent);
                        
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new UserFriendlyException($"Local AI hatası: {response.StatusCode}");
                        }

                        var responseString = await response.Content.ReadAsStringAsync();

                        //  Büyük/küçük harf duyarlılığını kapatıyoruz
                        var options = new JsonSerializerOptions
                        { 
                            PropertyNameCaseInsensitive = true
                        };
                        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseString, options);
                        return new AiAnswerDto 
                        { 
                            Answer = ollamaResponse?.Response ?? "Cevap alınamadı." 
                        };
                    }
                }
                catch (Exception ex)
                {
                     // Hata varsa hatayı görelim
                     return new AiAnswerDto 
                     { 
                         Answer = "HATA OLUŞTU: " + ex.Message 
                     };
                }
            }
        }
   
    public class OllamaResponse
        {
            [JsonPropertyName("response")]
            public string Response { get; set; } = string.Empty;
        }
    }
}