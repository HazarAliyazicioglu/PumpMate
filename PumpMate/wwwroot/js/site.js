// PumpMate Tema Değiştirme Sistemi

// Tema değiştirme fonksiyonu
function toggleTheme() {
    const html = document.documentElement;
    const currentTheme = html.getAttribute('data-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    
    // Tema değişkenini güncelle
    html.setAttribute('data-theme', newTheme);
    
    // LocalStorage'a kaydet
    localStorage.setItem('pumpmate-theme', newTheme);
    
    // Tema geçiş animasyonu için class ekle
    document.body.classList.add('theme-transition');
    
    // Animasyon class'ını kaldır
    setTimeout(() => {
        document.body.classList.remove('theme-transition');
    }, 300);
    
    // Tema değiştirme göstergesi
    showThemeIndicator(newTheme);
}

// Tema değiştirme göstergesi
function showThemeIndicator(theme) {
    // Mevcut göstergeleri kaldır
    const existingIndicator = document.querySelector('.theme-indicator');
    if (existingIndicator) {
        existingIndicator.remove();
    }
    
    // Yeni gösterge oluştur
    const indicator = document.createElement('div');
    indicator.className = 'theme-indicator';
    indicator.textContent = theme === 'dark' ? '🌙 Karanlık Tema Aktif' : '☀️ Açık Tema Aktif';
    
    document.body.appendChild(indicator);
    
    // Göstergeyi göster
    setTimeout(() => {
        indicator.classList.add('show');
    }, 100);
    
    // Göstergeyi gizle
    setTimeout(() => {
        indicator.classList.remove('show');
        setTimeout(() => {
            if (indicator.parentNode) {
                indicator.remove();
            }
        }, 300);
    }, 2000);
}

// Sayfa yüklendiğinde tema ayarlarını yükle
document.addEventListener('DOMContentLoaded', function() {
    // LocalStorage'dan tema ayarını al
    const savedTheme = localStorage.getItem('pumpmate-theme');
    
    if (savedTheme) {
        document.documentElement.setAttribute('data-theme', savedTheme);
    }
    
    // PumpMate yazısına tıklama olayı ekle
    const pumpMateBrand = document.querySelector('.navbar-brand');
    if (pumpMateBrand) {
        pumpMateBrand.classList.add('theme-toggle');
        pumpMateBrand.addEventListener('click', function(e) {
            e.preventDefault();
            toggleTheme();
        });
    }
    
    // Tüm linklerin href'lerini koru (tema değiştirme sırasında sayfa yenilenmesin)
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            if (this.classList.contains('theme-toggle')) {
                e.preventDefault();
                return;
            }
        });
    });
});

// Sistem teması değişikliğini dinle
if (window.matchMedia) {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    mediaQuery.addListener(function(e) {
        if (!localStorage.getItem('pumpmate-theme')) {
            const newTheme = e.matches ? 'dark' : 'light';
            document.documentElement.setAttribute('data-theme', newTheme);
        }
    });
}

// Tema değiştirme butonuna hover efekti
document.addEventListener('DOMContentLoaded', function() {
    const themeToggle = document.querySelector('.navbar-brand.theme-toggle');
    if (themeToggle) {
        themeToggle.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.1)';
        });
        
        themeToggle.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    }
});
