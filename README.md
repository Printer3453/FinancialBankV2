# FinancialBankV2 - Full-Stack Mini Bankacılık Portalı

Bu proje, modern .NET teknolojileri ve Angular kullanılarak geliştirilmiş, kişisel bir Full-Stack mini bankacılık uygulamasıdır. Uygulama, temel bankacılık özelliklerini, yapay zeka entegrasyonunu ve katmanlı mimari prensiplerini sergilemek amacıyla oluşturulmuştur.

##  Temel Özellikler

- **Kullanıcı Yönetimi:** ABP Framework'ün sunduğu hazır kimlik yönetimi modülü ile güvenli kullanıcı girişi ve kaydı.
- **Hesap Paneli:** Giriş yapan kullanıcıların kendilerine ait banka hesaplarını ve güncel bakiyelerini görüntülemesi.
- **Para Transferi:** Kullanıcıların, sistemdeki başka bir hesaba hesap numarasını kullanarak para transferi yapabilmesi.
- **İşlem Geçmişi:** Yapılan tüm transferlerin veritabanında bir `Transaction` kaydı olarak saklanması.
- **Yapay Zeka Asistanı:** OpenAI API entegrasyonu ile, kullanıcıların hesap bakiyeleri hakkında doğal dilde sorular sorabilmesi.
- **İşlem Dekontu:** Başarılı her para transferi sonrası, işlem detaylarını (gönderen, alıcı, tutar, tarih) gösteren dinamik bir dekont sayfası.

##  Kullanılan Teknolojiler

### Backend
- **Framework:** ABP Framework v9.3.5, ASP.NET Core
- **Mimari:** Katmanlı Mimari, Domain-Driven Design (DDD) Prensipleri
- **API:** RESTful API
- **ORM:** Entity Framework Core
- **Veritabanı:** Microsoft SQL Server
- **Harici API:** OpenAI API
- **Diğer:** AutoMapper, Dependency Injection, Unit of Work

### Frontend
- **Framework:** Angular
- **UI Kütüphanesi:** Bootstrap, FontAwesome
- **Dil:** TypeScript

##  Projeyi Çalıştırma

Projeyi yerel makinede çalıştırmak için aşağıdaki adımlar izlenmelidir:

1.  **Veritabanı Kurulumu:**
    - `appsettings.json` dosyalarındaki `ConnectionStrings` bölümünü kendi SQL Server bilgilerinize göre güncelleyin.
    - `src/FinancialBankV2.DbMigrator` projesini çalıştırarak veritabanını ve başlangıç verilerini oluşturun.

2.  **Backend'i Çalıştırma:**
    - `src/FinancialBankV2.HttpApi.Host` projesini `dotnet run` komutuyla çalıştırın. API `https://localhost:44363` adresinde çalışmaya başlayacaktır.

3.  **Frontend'i Çalıştırma:**
    - `angular` klasörüne gidin ve `yarn` komutuyla paketleri yükleyin.
    - `ng serve` komutuyla frontend sunucusunu başlatın. Uygulama `http://localhost:4200` adresinde çalışmaya başlayacaktır.

##  Standart Kullanıcı Bilgileri

Uygulamayı test etmek için aşağıdaki standart admin kullanıcısıyla giriş yapabilirsiniz:
- **Kullanıcı Adı:** `admin`
- **Şifre:** `1q2w3E*`
---

*Bu proje, Ömer Faruk YAZICI tarafından geliştirilmiştir.*

<img width="1600" height="796" alt="image" src="https://github.com/user-attachments/assets/78f42b71-7319-4773-9735-ff1613a7b89e" />
<img width="1600" height="820" alt="image" src="https://github.com/user-attachments/assets/2c6b7f6c-503d-49fc-a236-e661e28949d1" />
<img width="1600" height="712" alt="image" src="https://github.com/user-attachments/assets/f1865308-0603-4f42-b359-fb31538e0678" />
<img width="1600" height="758" alt="image" src="https://github.com/user-attachments/assets/3df49695-d3c5-448d-be8f-0166bda3a316" />
<img width="1600" height="756" alt="image" src="https://github.com/user-attachments/assets/5ffbfafe-3847-42c0-8a24-eb66405ebc8b" />




