# 🏋️ PumpMate - Fitness Tracking Application

PumpMate, kullanıcıların antrenman ve kalori takibini yapabilecekleri kapsamlı bir web uygulamasıdır.

## ✨ Özellikler

- 🔐 **Güvenli Kullanıcı Kimlik Doğrulama**
- 💪 **Antrenman Takibi** (Gym, Home, Cardio, Yoga)
- 🔥 **Kalori Hesaplama** (Alım ve Yakım)
- 📊 **İstatistikler ve Raporlama**
- 🌙 **Dark/Light Mode Desteği**
- 📱 **Responsive Tasarım**
- 🎯 **Kişiselleştirilmiş Fitness Rehberliği**

## 🚀 Hızlı Başlangıç

### Yöntem 1: Batch Dosyası ile (Önerilen)
1. `run-project.bat` dosyasına çift tıklayın
2. Uygulama otomatik olarak başlayacak
3. Tarayıcınızda `https://localhost:7001` adresine gidin

### Yöntem 2: Manuel Kurulum
```bash
# Bağımlılıkları yükle
dotnet restore

# Veritabanını güncelle
dotnet ef database update

# Uygulamayı çalıştır
dotnet run
```

## 📋 Gereksinimler

- .NET 6.0 veya üzeri
- SQL Server (LocalDB dahil)
- Modern web tarayıcısı

## 🛠️ Teknolojiler

- **Backend**: ASP.NET Core MVC
- **Veritabanı**: Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript
- **UI Framework**: Bootstrap 5
- **Tema**: Dark/Light Mode CSS Variables

## 📁 Proje Yapısı

```
PumpMate/
├── Controllers/          # İş mantığı
├── Models/              # Veri modelleri
├── Views/               # Kullanıcı arayüzü
├── Data/                # Veritabanı bağlantısı
├── Migrations/          # Veritabanı şemaları
├── wwwroot/             # Statik dosyalar
├── run-project.bat      # Hızlı başlatma
└── README.md           # Bu dosya
```

## 🎮 Kullanım

1. **Kayıt Ol**: Yeni hesap oluşturun
2. **Giriş Yap**: Hesabınıza giriş yapın
3. **Antrenman Ekle**: Egzersizlerinizi kaydedin
4. **Kalori Takip**: Günlük kalori alımınızı girin
5. **İstatistikleri İncele**: İlerlemenizi takip edin

## 🔧 Geliştirme

### Yeni Özellik Ekleme
1. Model sınıfını oluşturun
2. Controller ekleyin
3. View dosyalarını oluşturun
4. Migration ekleyin: `dotnet ef migrations add FeatureName`

### Veritabanı Güncelleme
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## 📞 Destek

Herhangi bir sorun yaşarsanız:
- GitHub Issues kullanın
- Proje dokümantasyonunu inceleyin

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

---

**PumpMate ile fitness yolculuğunuzu takip edin! 💪** 