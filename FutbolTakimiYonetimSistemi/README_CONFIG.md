# Konfigürasyon Kurulum Kılavuzu

## 🔧 App.config Ayarlama

### Adım 1: Config Dosyasını Kopyalayın
```bash
# App.config.example dosyasını kopyalayın
copy App.config.example App.config
```

### Adım 2: PostgreSQL Şifrenizi Girin
`App.config` dosyasını açın ve şu satırı bulun:
```xml
Password=YOUR_PASSWORD_HERE;
```

Bunu kendi PostgreSQL şifrenizle değiştirin:
```xml
Password=rambo1234;
```

### Adım 3: Kaydedin ve Çalıştırın
Artık projeyi çalıştırabilirsiniz!

## ⚠️ ÖNEMLİ GÜVENLİK UYARILARI

### Git ile Çalışırken
- ✅ `App.config.example` → Git'e eklenebilir (şifre yok)
- ❌ `App.config` → GİT'E EKLEMEYİN! (.gitignore'da)

### Kontrol Etme
```bash
# App.config'in git'e eklenmediğini doğrulayın
git status

# App.config görünmemeli!
```

## 🔐 Varsayılan Ayarlar

| Ayar | Değer | Açıklama |
|------|-------|----------|
| Host | localhost | Veritabanı sunucusu |
| Port | 5432 | PostgreSQL portu |
| Database | futbol_takimi_db | Veritabanı adı |
| Username | postgres | Kullanıcı adı |
| Password | YOUR_PASSWORD_HERE | ⚠️ DEĞİŞTİRİN! |
| PasswordHashWorkFactor | 11 | BCrypt güvenlik seviyesi |
| SessionTimeout | 60 dakika | Oturum süresi |

## 🛠️ Farklı Ortamlar İçin

### Geliştirme (Development)
```xml
<add key="Version" value="2.0 - Development" />
```

### Üretim (Production)
```xml
<add key="Version" value="2.0 - Production" />
<!-- Daha güçlü şifre kullanın! -->
<!-- SessionTimeoutMinutes'i azaltın -->
```

## 📞 Sorun mu Var?

### "Connection Failed" Hatası
1. PostgreSQL servisi çalışıyor mu? (services.msc)
2. Şifre doğru mu?
3. Veritabanı oluşturuldu mu? (`futbol_takimi_db`)

### "Config dosyası bulunamadı"
1. `App.config` dosyasını oluşturdunuz mu?
2. Dosya doğru klasörde mi? (`FutbolTakimiYonetimSistemi/App.config`)

---

**Son Güncelleme:** 22 Aralık 2024

