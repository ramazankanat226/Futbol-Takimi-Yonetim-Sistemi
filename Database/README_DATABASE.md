# PostgreSQL Veritabanı Kurulum Kılavuzu

## ⚡ Hızlı Başlangıç (2 Dakika) - TEK DOSYA İLE KURULUM

**En kolay yol:**

1. **PostgreSQL 14** kurun (Sonraki bölümde detaylar)
2. **pgAdmin 4** açın
3. **futbol_takimi_db** veritabanı oluşturun
4. **Query Tool** açın
5. **`Database\Schema\00_TumKurulum_Full.sql`** dosyasını aç
6. **F5** tuşuna bas (30 saniye bekle)
7. ✅ **TAMAMDIR!**

**Test Kullanıcıları:**
- Yönetici: `admin` / `admin123`
- Antrenör: `teknikdirektor` / `antrenor123`

---

### 📦 İçerik (Tek Dosyada)

`00_TumKurulum_Full.sql` dosyası şunları içerir:
- ✅ 7 Tablo
- ✅ 13 Index (performans için)
- ✅ 10 Stored Procedure (CRUD işlemleri)
- ✅ 4 Trigger (log tutma, uyarılar)
- ✅ 10 Futbolcu + 3 Antrenör test verisi
- ✅ Normalizasyon 3NF

Detaylı adımlar aşağıda ⬇️

---

## 📋 İçindekiler
1. [Gereksinimler](#gereksinimler)
2. [PostgreSQL Kurulumu](#postgresql-kurulumu)
3. [Veritabanı Oluşturma](#veritabanı-oluşturma)
4. [Şema Yükleme](#şema-yükleme-manuel-yöntem-önerilen)
5. [Test ve Doğrulama](#test-ve-doğrulama)
6. [Bağlantı Bilgileri](#bağlantı-bilgileri)
7. [Sorun Giderme](#sorun-giderme)

---

## 🔧 Gereksinimler

### Sistem Gereksinimleri
- **İşletim Sistemi**: Windows 10/11, Linux, macOS
- **RAM**: Minimum 2 GB (4 GB önerilir)
- **Disk Alanı**: Minimum 500 MB
- **PostgreSQL**: Sürüm 14.x veya üzeri

### Yazılım Gereksinimleri
- PostgreSQL 14+ 
- pgAdmin 4 (grafik arayüz için, opsiyonel)
- psql komut satırı aracı (PostgreSQL ile gelir)

---

## 📥 PostgreSQL Kurulumu

### Windows İçin

1. **PostgreSQL İndirme**
   - [PostgreSQL Resmi İndirme Sayfası](https://www.postgresql.org/download/windows/)
   - EnterpriseDB installer'ı indirin (en kolay yöntem)

2. **Kurulum Adımları**
   ```
   1. İndirilen .exe dosyasını çalıştırın
   2. Kurulum dizinini seçin (varsayılan: C:\Program Files\PostgreSQL\14)
   3. Bileşenleri seçin (hepsini seçin)
   4. Veri dizinini seçin (varsayılan: C:\Program Files\PostgreSQL\14\data)
   5. Superuser (postgres) şifresini belirleyin
      ⚠️ ÖNEMLİ: Bu şifreyi unutmayın!
   6. Port numarasını seçin (varsayılan: 5432)
   7. Locale ayarını seçin (Turkish, Turkey veya Default locale)
   8. Kurulumu tamamlayın
   ```

3. **Stack Builder** (opsiyonel)
   - Kurulum sonrası Stack Builder çalışır
   - pgAdmin 4 otomatik yüklüdür
   - İptal edebilirsiniz

### Linux İçin (Ubuntu/Debian)

```bash
# PostgreSQL deposunu ekle
sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -

# Güncelleme ve kurulum
sudo apt-get update
sudo apt-get install postgresql-14 postgresql-contrib-14

# PostgreSQL başlat
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Şifre belirleme
sudo -u postgres psql
ALTER USER postgres PASSWORD 'yourpassword';
\q
```

### macOS İçin

```bash
# Homebrew ile kurulum
brew install postgresql@14

# Servisi başlat
brew services start postgresql@14

# Şifre belirleme
psql postgres
ALTER USER postgres PASSWORD 'yourpassword';
\q
```

---

## 🗄️ Veritabanı Oluşturma

### Yöntem 1: psql Komut Satırı (Önerilen)

1. **psql'e Bağlan**
   ```bash
   # Windows
   "C:\Program Files\PostgreSQL\14\bin\psql.exe" -U postgres

   # Linux/macOS
   psql -U postgres
   ```

2. **Şifrenizi Girin**
   - Kurulum sırasında belirlediğiniz şifreyi girin

3. **Veritabanı Oluştur**
   ```sql
   CREATE DATABASE futbol_takimi_db
       WITH 
       OWNER = postgres
       ENCODING = 'UTF8'
       LC_COLLATE = 'Turkish_Turkey.1254'
       LC_CTYPE = 'Turkish_Turkey.1254'
       TABLESPACE = pg_default
       CONNECTION LIMIT = -1;
   ```

4. **Veritabanına Bağlan**
   ```sql
   \c futbol_takimi_db
   ```

### Yöntem 2: pgAdmin 4 (Grafik Arayüz)

1. **pgAdmin 4'ü Aç**
   - Start Menu > PostgreSQL > pgAdmin 4

2. **Sunucuya Bağlan**
   - Sol panelde: Servers > PostgreSQL 14
   - Şifrenizi girin

3. **Yeni Veritabanı Oluştur**
   ```
   Sağ tık: Databases > Create > Database
   
   Genel:
   - Database: futbol_takimi_db
   - Owner: postgres
   
   Definition:
   - Encoding: UTF8
   - Collation: Turkish_Turkey.1254
   - Character type: Turkish_Turkey.1254
   
   [Save] butonuna tıklayın
   ```

---

## 📝 Şema Yükleme - Manuel Yöntemler

### ⭐ YÖNTEM 1: Tek Dosya ile Tam Kurulum (ÖNERİLEN)

**📌 pgAdmin 4 Hızlı Kılavuz:**

```
1. Start Menu > PostgreSQL > pgAdmin 4
2. Sol Panel > Servers > PostgreSQL 14 > Databases > futbol_takimi_db
3. Sağ tık > Query Tool
4. Üstteki Klasör ikonu 📁 (veya File > Open - Ctrl+O)
5. 00_TumKurulum_Full.sql dosyasını seç
6. ▶ Play butonu (veya F5)
7. ~30 saniye bekle
```

---

#### Adım 1: pgAdmin 4'ü Açın ve Query Tool Başlatın

1. **Start Menu** > **PostgreSQL** > **pgAdmin 4**
2. Sol panelde: **Servers** > **PostgreSQL 14** > **Databases** > **futbol_takimi_db**
3. **Sağ tıklayın** > **Query Tool** seçin

#### Adım 2: Tek SQL Dosyasını Çalıştırın

1. Query Tool penceresinde **Klasör ikonu 📁** tıklayın (veya **Ctrl+O**)
2. Dosyayı seçin: `C:\Users\USER\Desktop\Futbol_Takimi\Database\Schema\00_TumKurulum_Full.sql`
3. **▶ Play butonu** veya **F5** tuşuna basın
4. **30 saniye** bekleyin (tüm kurulum yapılıyor)

**Beklenen Çıktı:**
```
NOTICE:  ╔════════════════════════════════════════════════╗
NOTICE:  ║  KURULUM BAŞARIYLA TAMAMLANDI!               ║
NOTICE:  ╠════════════════════════════════════════════════╣
NOTICE:  ║  Tablo Sayısı       : 7                       ║
NOTICE:  ║  Futbolcu Sayısı    : 10                      ║
NOTICE:  ║  Antrenör Sayısı    : 3                       ║
NOTICE:  ║  Stored Procedure   : 10 adet                 ║
NOTICE:  ║  Trigger            : 4 adet                  ║
NOTICE:  ║  Index              : 13 adet                 ║
NOTICE:  ╠════════════════════════════════════════════════╣
NOTICE:  ║  TEST KULLANICILARI:                          ║
NOTICE:  ║  Yönetici : admin / admin123                  ║
NOTICE:  ║  Antrenör : teknikdirektor / antrenor123      ║
NOTICE:  ╚════════════════════════════════════════════════╝
Query returned successfully in 500-1000 msec.
```

#### Adım 3: Kurulumu Doğrulayın

Query Tool'da şu sorguları çalıştırın:

```sql
-- Tabloları listele
SELECT table_name FROM information_schema.tables 
WHERE table_schema = 'public' ORDER BY table_name;

-- Veri sayılarını kontrol et
SELECT 'Yöneticiler' as Tablo, COUNT(*) as Sayi FROM Yoneticiler
UNION ALL
SELECT 'Antrenörler', COUNT(*) FROM Antrenorler
UNION ALL
SELECT 'Futbolcular', COUNT(*) FROM Futbolcular
UNION ALL
SELECT 'Antrenmanlar', COUNT(*) FROM Antrenmanlar;
```

---

### 📌 YÖNTEM 2: psql ile Kopyala-Yapıştır (Alternatif)

**Eğer pgAdmin kullanmak istemiyorsanız:**

1. **Start Menu** > **PostgreSQL 14** > **SQL Shell (psql)**
2. Enter, Enter, Enter basıp şifrenizi girin
3. `\c futbol_takimi_db` yazıp Enter
4. **Not Defteri** ile `00_TumKurulum_Full.sql` dosyasını açın
5. **Tüm içeriği** kopyalayın (Ctrl+A, Ctrl+C)
6. psql penceresine **sağ tık** > **Paste** (veya Shift+Insert)
7. Enter tuşuna basın ve bekleyin

⚠️ **Not**: psql penceresine uzun kod yapıştırırken donmuş gibi görünebilir, bu normaldir.

---

## ✅ Test ve Doğrulama

### Veritabanı Bağlantı Testi

```sql
-- psql ile bağlan
psql -U postgres -d futbol_takimi_db

-- Tabloları listele
\dt

-- Beklenen çıktı:
--              List of relations
--  Schema |        Name        | Type  |  Owner   
-- --------+--------------------+-------+----------
--  public | antrenmanlar       | table | postgres
--  public | antrenorler        | table | postgres
--  public | bildirimler        | table | postgres
--  public | futbolcuantrenman  | table | postgres
--  public | futbolcular        | table | postgres
--  public | logtablosu         | table | postgres
--  public | yoneticiler        | table | postgres
```

### Veri Kontrolü

```sql
-- Yönetici sayısı
SELECT COUNT(*) FROM Yoneticiler;
-- Beklenen: 3

-- Antrenör sayısı
SELECT COUNT(*) FROM Antrenorler;
-- Beklenen: 4

-- Futbolcu sayısı
SELECT COUNT(*) FROM Futbolcular;
-- Beklenen: 17

-- Aktif futbolcu sayısı
SELECT COUNT(*) FROM Futbolcular WHERE Durumu = 'Aktif';
-- Beklenen: 14+

-- Antrenman sayısı
SELECT COUNT(*) FROM Antrenmanlar;
-- Beklenen: 14
```

### Test Kullanıcıları ile Giriş

```sql
-- Yönetici giriş testi
SELECT * FROM Yoneticiler WHERE KullaniciAdi = 'admin';

-- Antrenör giriş testi
SELECT * FROM Antrenorler WHERE KullaniciAdi = 'teknikdirektor';
```

---

## 🔐 Bağlantı Bilgileri

### C# Connection String

```csharp
// appsettings.json veya App.config
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=futbol_takimi_db;Username=postgres;Password=yourpassword;"
  }
}
```

### Npgsql ile Bağlantı (C#)

```csharp
using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=futbol_takimi_db;Username=postgres;Password=yourpassword;";

using (var conn = new NpgsqlConnection(connectionString))
{
    conn.Open();
    Console.WriteLine("Bağlantı başarılı!");
    
    // Test sorgusu
    using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Futbolcular", conn))
    {
        var count = cmd.ExecuteScalar();
        Console.WriteLine($"Futbolcu sayısı: {count}");
    }
}
```

### Test Kullanıcı Bilgileri

| Kullanıcı Tipi | Kullanıcı Adı | Şifre | Rol |
|----------------|---------------|-------|-----|
| Yönetici | admin | admin123 | Admin (Tam Yetki) |
| Yönetici | yonetici1 | admin123 | Yönetici |
| Antrenör | teknikdirektor | antrenor123 | Teknik Direktör |
| Antrenör | antrenor_kondisyon | antrenor123 | Kondisyon Antrenörü |
| Antrenör | antrenor_teknik | antrenor123 | Teknik Antrenör |
| Antrenör | antrenor_taktik | antrenor123 | Taktik Antrenörü |

⚠️ **GÜVENLİK UYARISI**: Bu şifreler test içindir. Üretim ortamında mutlaka değiştirin!

---

## 🐛 Sorun Giderme

### Sorun 1: "psql command not found" veya psql açılmıyor

**Çözüm:**
- Windows Start Menu'den **"SQL Shell (psql)"** programını arayın
- Veya **pgAdmin 4** kullanın (daha kolay)

### Sorun 2: "FATAL: password authentication failed"

**Çözüm:**
```sql
-- postgres kullanıcısı ile bağlanın
psql -U postgres

-- Şifreyi sıfırlayın
ALTER USER postgres PASSWORD 'newpassword';
```

### Sorun 3: "database does not exist"

**Çözüm:**
```bash
# Önce postgres veritabanına bağlanın
psql -U postgres -d postgres

# Veritabanı var mı kontrol edin
\l

# Yoksa oluşturun
CREATE DATABASE futbol_takimi_db;
```

### Sorun 4: Encoding/Collation Hatası

**Çözüm:**
```sql
-- Veritabanını silerken template kullan
DROP DATABASE IF EXISTS futbol_takimi_db;

CREATE DATABASE futbol_takimi_db
    WITH 
    TEMPLATE = template0
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C';
```

### Sorun 5: Constraint Violation

**Çözüm:**
```sql
-- Constraint'leri kontrol et
SELECT * FROM information_schema.table_constraints 
WHERE table_name = 'futbolcular';

-- Sorunlu constraint'i geçici olarak devre dışı bırak
ALTER TABLE Futbolcular DISABLE TRIGGER ALL;
-- Veri yükle
ALTER TABLE Futbolcular ENABLE TRIGGER ALL;
```

### Sorun 6: PostgreSQL Servisi Çalışmıyor

**Çözüm (Windows):**

1. **Windows Hizmetler** (Services) uygulamasını açın:
   - Start Menu'de "services.msc" yazın
   
2. Listede **"postgresql-x64-14"** servisini bulun

3. **Sağ tıklayın** > **Start** (veya Restart)

**Alternatif:**
- pgAdmin 4 açıldığında servisi otomatik başlatır
- Bilgisayarı yeniden başlatın

---

## 📊 Veritabanı Bakım

### Düzenli Bakım (pgAdmin 4 ile)

**Manuel Bakım:**

1. **pgAdmin 4** açın
2. **futbol_takimi_db** veritabanına **sağ tıklayın**
3. **Maintenance...** seçeneğini seçin
4. **VACUUM** ve **ANALYZE** seçeneklerini işaretleyin
5. **OK** butonuna tıklayın

**SQL ile Bakım (Query Tool):**

pgAdmin 4 > Query Tool'da şu komutları çalıştırın:

```sql
-- Veritabanı istatistiklerini güncelle
ANALYZE;

-- Ölü satırları temizle
VACUUM;

-- Tam temizlik ve istatistik güncelleme
VACUUM ANALYZE;

-- Tablo boyutlarını kontrol et
SELECT 
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

### Yedekleme (pgAdmin 4 ile)

1. **pgAdmin 4** açın
2. Sol panelde **futbol_takimi_db** veritabanına **sağ tıklayın**
3. **Backup...** seçeneğini seçin
4. **Filename** bölümünde kayıt yeri seçin (örn: `C:\Backups\futbol_takimi_backup.backup`)
5. **Format**: Custom veya Plain (SQL) seçin
6. **Backup** butonuna tıklayın

**Otomatik yedekleme ayarlayabilirsiniz:**
- Format: **Plain** (okunabilir SQL dosyası)
- **Data/Objects** sekmesinde neyi yedekleyeceğinizi seçin

### Geri Yükleme (pgAdmin 4 ile)

1. **pgAdmin 4** açın
2. Eğer veritabanı varsa silin, yoksa yeni oluşturun
3. **futbol_takimi_db** veritabanına **sağ tıklayın**
4. **Restore...** seçeneğini seçin
5. **Filename** bölümünde backup dosyasını seçin
6. **Restore** butonuna tıklayın

**SQL dosyasından geri yükleme:**
- Query Tool açın
- **File > Open** ile SQL dosyasını açın
- **Execute** (F5) tuşuna basın

---

## 📞 Destek ve İletişim

### Yararlı Kaynaklar
- [PostgreSQL Resmi Dokümantasyon](https://www.postgresql.org/docs/)
- [Npgsql Dokümantasyon](https://www.npgsql.org/doc/)
- [pgAdmin Dokümantasyon](https://www.pgadmin.org/docs/)

### Hata Raporlama
- GitHub Issues: Projenin GitHub sayfasında issue açın
- E-posta: Proje yöneticisine ulaşın

---

## ✅ Kurulum Tamamlandı!

Veritabanınız artık kullanıma hazır. C# uygulamasını çalıştırabilirsiniz.

**Sonraki Adım**: Stored Procedure ve Trigger dosyalarını yükleyin (Database/StoredProcedures ve Database/Triggers klasörleri).

---

**Son Güncelleme**: 3 Aralık 2025  
**Versiyon**: 1.0  
**Proje**: Futbol Takımı Yönetim Sistemi

