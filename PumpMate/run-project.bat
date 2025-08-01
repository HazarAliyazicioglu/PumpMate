@echo off
title PumpMate - Fitness Tracking Application
color 0A

echo.
echo ========================================
echo    🏋️ PumpMate Fitness Tracker
echo ========================================
echo.
echo 🚀 Proje başlatılıyor...
echo.

:: Proje dizinine git
cd /d "%~dp0"

:: .NET SDK'nın yüklü olup olmadığını kontrol et
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ HATA: .NET SDK bulunamadı!
    echo.
    echo 📥 Lütfen .NET 6.0 veya üzerini yükleyin:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

:: Bağımlılıkları yükle
echo 📦 Bağımlılıklar yükleniyor...
dotnet restore
if %errorlevel% neq 0 (
    echo ❌ HATA: Bağımlılıklar yüklenemedi!
    pause
    exit /b 1
)

:: Veritabanını güncelle
echo 🗄️ Veritabanı güncelleniyor...
dotnet ef database update
if %errorlevel% neq 0 (
    echo ⚠️ UYARI: Veritabanı güncellenemedi, devam ediliyor...
)

:: Projeyi çalıştır
echo.
echo 🌐 Web uygulaması başlatılıyor...
echo 📱 Tarayıcınızda otomatik olarak açılacak
echo 🔗 HTTPS URL: https://localhost:7199
echo 🔗 HTTP URL: http://localhost:5174
echo.
echo ⏹️ Durdurmak için Ctrl+C tuşlayın
echo.

:: 3 saniye bekle ve tarayıcıyı aç
timeout /t 3 /nobreak >nul
start https://localhost:7199

:: Projeyi çalıştır
dotnet run

echo.
echo 👋 PumpMate kapatıldı.
pause 