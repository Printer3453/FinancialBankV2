# FinancialBankV2 - Mini Bankacılık Portalı

Bu proje, ABP Framework ve Angular kullanılarak geliştirilmiş bir Full-Stack mini bankacılık uygulamasıdır. 
Uygulama, temel bankacılık özelliklerini (hesap görüntüleme, para transferi) içermekte ve 
 .NET teknolojileri üzerine kurulmuştur.

## Temel Özellikler

- **Kullanıcı Yönetimi:** ABP Framework'ün sunduğu hazır kimlik yönetimi modülü ile güvenli kullanıcı girişi ve kaydı.
- **Hesap Paneli:** Giriş yapan kullanıcıların kendilerine ait banka hesaplarını ve güncel bakiyelerini görüntülemesi.
- **Para Transferi:** Kullanıcıların, sistemdeki başka bir hesaba hesap numarasını kullanarak para transferi yapabilmesi.
- **İşlem Geçmişi:** Yapılan tüm transferlerin veritabanında bir `Transaction` kaydı olarak saklanması.

## Kullanılan Teknolojiler

### Backend
- **Framework:** ABP Framework v9.3.5, ASP.NET Core
- **Mimari:** Katmanlı Mimari, Domain-Driven Design (DDD) Prensipleri
- **API:** RESTful API
- **ORM:** Entity Framework Core
- **Veritabanı:** Microsoft SQL Server
- **Diğer:** AutoMapper, Dependency Injection, Unit of Work

### Frontend
- **Framework:** Angular
- **UI Kütüphanesi:** Bootstrap, FontAwesome
- **Dil:** TypeScript

## Projeyi Çalıştırma

Projeyi yerel makinede çalıştırmak için aşağıdaki adımlar izlenmelidir:

1.  **Veritabanı Kurulumu:**
    - `appsettings.json` dosyalarındaki `ConnectionStrings` bölümünü kendi SQL Server bilgilerinize göre güncelleyin.
    - `src/FinancialBankV2.DbMigrator` projesini çalıştırarak veritabanını ve başlangıç verilerini oluşturun.

2.  **Backend'i Çalıştırma:**
    - `src/FinancialBankV2.HttpApi.Host` projesini `dotnet run` komutuyla çalıştırın. API `https://localhost:44363` adresinde çalışmaya başlayacaktır.

3.  **Frontend'i Çalıştırma:**
    - `angular` klasörüne gidin ve `yarn` komutuyla paketleri yükleyin.
    - `ng serve` komutuyla frontend sunucusunu başlatın. Uygulama `http://localhost:4200` adresinde çalışmaya başlayacaktır.

## Standart Kullanıcı Bilgileri

Uygulamayı test etmek için aşağıdaki standart admin kullanıcısıyla giriş yapabilirsiniz:
- **Kullanıcı Adı:** `admin`
- **Şifre:** `1q2w3E*`

---

*Bu proje, Ömer Faruk YAZICI tarafından geliştirilmiştir.*
