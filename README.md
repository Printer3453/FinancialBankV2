# FinancialBankV2 — Full-Stack Bankacılık Simülasyonu

Modern **.NET (ABP Framework)** backend ve **Angular** frontend ile geliştirilmiş, temel internet bankacılığı akışlarını ve **yerel LLM tabanlı bir AI asistanını** gösteren kişisel bir full-stack projesidir.

## Özellikler

- **Kullanıcı yönetimi:** ABP Framework'ün hazır Identity modülüyle giriş/kayıt.
- **Hesap paneli:** Giriş yapan kullanıcı kendi hesaplarını ve bakiyelerini görür.
- **Para transferi:** Hesap numarasıyla sistem içi transfer.
- **İşlem geçmişi:** Transferler kalıcı `Transaction` kaydı olarak tutulur; sunucu tarafı sayfalama ile listelenir.
- **AI asistanı (yerel, gizlilik odaklı):** Semantic Kernel + yerel LLM (Ollama) ile; kullanıcı doğal dille "ne kadar param var / son harcamalarım" diye sorabilir. **İki aşamalı çalışır:** önce soruyu sınıflandırır ([BAKIYE]/[ISLEM]/[SOHBET]), sonra yalnızca ilgili kullanıcı verisini bağlam olarak modele verir. Prompt'ta "veri yoksa uydurma" tarzı anti-halüsinasyon kuralları var ve yalnızca kullanıcının kendi verisi çekilir.
- **Dekont:** Başarılı transfer sonrası işlem detaylarını gösteren dekont sayfası.

## Teknoloji Yığını

**Backend:** ABP Framework 9.3.5, ASP.NET Core, EF Core + SQL Server, Semantic Kernel (yerel LLM), AutoMapper, DI, Unit of Work
**Frontend:** Angular 20 (standalone component), ABP proxy, Bootstrap, ngx-toastr, TypeScript
**Mimari:** ABP katmanlı mimari

## Bilinen Sınırlar / Yol Haritası 

Para söz konusu olduğu için en önemlisi ilk sırada:

- **Eşzamanlılık kontrolü yok:** `CreateTransactionAsync` bakiyeyi oku→kontrol et→güncelle akışıyla çalışıyor; iki eşzamanlı transfer "lost update" yaratabilir. Doğrusu optimistic concurrency (rowversion) veya transaction içinde domain invariant. (Üretim için kritik — eklenecek.)
- **Anemik domain:** `BankAccount`/`Transaction` davranışsız veri sınıfları; bakiye düşürme mantığı app service'te. `BankAccount.Withdraw(...)` gibi davranışsal metotlara taşınmalı.
- `GetTransactionById` çağrısına kayıt sahipliği (yetki) kontrolü eklenecek.
- AI asistanının testleri canlı Ollama'ya bağlı; mock'lanıp CI'da deterministik hale getirilecek.
- LLM endpoint/model bilgisi yapılandırmaya taşınacak; finansal kayıtlarda `DateTime.Now` yerine UTC kullanılacak.

## Çalıştırma

1. **Veritabanı:** `appsettings.json`'daki `ConnectionStrings`'i güncelle; `src/FinancialBankV2.DbMigrator`'ı çalıştırıp migrasyon + seed uygula.
2. **Backend:** `src/FinancialBankV2.HttpApi.Host` → `dotnet run` (API `https://localhost:44363`).
3. **Frontend:** `angular` klasöründe `yarn` ile bağımlılıkları kur, `ng serve` ile başlat (`http://localhost:4200`).
4. **AI asistanı:** Ollama'yı yerelde çalıştır.

> Varsayılan test kullanıcısı ABP'nin standart `admin` hesabıdır; üretimde değiştirilmelidir.

---
*Geliştirici: Ömer Faruk Yazıcı · Full-stack öğrenme projesidir.*


<img width="1600" height="796" alt="image" src="https://github.com/user-attachments/assets/78f42b71-7319-4773-9735-ff1613a7b89e" />
<img width="1600" height="820" alt="image" src="https://github.com/user-attachments/assets/2c6b7f6c-503d-49fc-a236-e661e28949d1" />
<img width="1600" height="712" alt="image" src="https://github.com/user-attachments/assets/f1865308-0603-4f42-b359-fb31538e0678" />
<img width="1600" height="758" alt="image" src="https://github.com/user-attachments/assets/3df49695-d3c5-448d-be8f-0166bda3a316" />
<img width="1600" height="756" alt="image" src="https://github.com/user-attachments/assets/5ffbfafe-3847-42c0-8a24-eb66405ebc8b" />




