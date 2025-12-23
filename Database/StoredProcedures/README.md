# 🔐 STORED PROCEDURES

Veritabanında 10 adet stored procedure bulunmaktadır.

## 📋 Kategoriler

### 1️⃣ Giriş ve Yetkilendirme (2 adet)

#### `sp_yonetici_giris`
**Parametre:** kullanici_adi, sifre  
**Dönüş:** Yönetici bilgileri veya NULL

```sql
-- Kullanım:
SELECT * FROM sp_yonetici_giris('semih', 'semih123');

-- Başarılı giriş:
-- YoneticiID | KullaniciAdi | Ad | Soyad | KullaniciTipi
```

#### `sp_antrenor_giris`
**Parametre:** kullanici_adi, sifre  
**Dönüş:** Antrenör bilgileri veya NULL

```sql
-- Kullanım:
SELECT * FROM sp_antrenor_giris('ramazan', 'ramazan123');
```

---

### 2️⃣ Futbolcu CRUD İşlemleri (4 adet)

#### `sp_futbolcu_ekle`
**Parametre:** Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, FormaNo, ...  
**Dönüş:** Yeni FutbolcuID

```sql
-- Kullanım:
SELECT sp_futbolcu_ekle(
    'Ahmet', 'Yılmaz', '1995-05-15', 
    180, 75, 'Orta Saha', 10, 
    50000, '2024-01-01', '2026-12-31', 
    'Türkiye', 'Aktif', 'Genç yetenek'
);
```

#### `sp_futbolcu_guncelle`
**Parametre:** FutbolcuID + güncellenecek alanlar  
**Dönüş:** Başarı durumu

```sql
-- Kullanım:
SELECT sp_futbolcu_guncelle(
    1,                    -- FutbolcuID
    'Ahmet', 'Kaya',     -- Ad, Soyad
    '1995-05-15',         -- DogumTarihi
    182, 78,              -- Boy, Kilo
    'Orta Saha', 10,      -- Pozisyon, FormaNo
    75000,                -- Maas
    '2024-01-01', '2027-12-31',  -- Sözleşme
    'Türkiye', 'Aktif',   -- Uyruk, Durumu
    'Performans artışı'   -- Notlar
);
```

#### `sp_futbolcu_sil`
**Parametre:** FutbolcuID  
**Dönüş:** Başarı mesajı

```sql
-- Kullanım:
SELECT sp_futbolcu_sil(1);
```

#### `sp_futbolcu_listele`
**Parametre:** arama_metni (opsiyonel)  
**Dönüş:** Futbolcu listesi

```sql
-- Tüm futbolcular:
SELECT * FROM sp_futbolcu_listele('');

-- Arama ile:
SELECT * FROM sp_futbolcu_listele('Ahmet');
```

---

### 3️⃣ Antrenman Yönetimi (2 adet)

#### `sp_antrenman_ekle`
**Parametre:** Tarih, BaslangicSaati, BitisSaati, Tur, Notlar, AntrenorID  
**Dönüş:** Yeni AntrenmanID

```sql
-- Kullanım:
SELECT sp_antrenman_ekle(
    '2024-12-25',         -- Tarih
    '10:00', '12:00',     -- Saat aralığı
    'Kondisyon',          -- Tür
    'Sprint çalışması',   -- Notlar
    1                     -- AntrenorID
);
```

#### `sp_katilim_ekle`
**Parametre:** FutbolcuID, AntrenmanID, Katilim, Performans, Notlar  
**Dönüş:** Başarı durumu

```sql
-- Kullanım:
SELECT sp_katilim_ekle(
    1,                    -- FutbolcuID
    5,                    -- AntrenmanID
    TRUE,                 -- Katıldı mı?
    8,                    -- Performans (1-10)
    'İyi performans'      -- Notlar
);
```

---

### 4️⃣ Raporlama (2 adet)

#### `sp_performans_raporu`
**Parametre:** -  
**Dönüş:** Futbolcu performans istatistikleri

```sql
-- Kullanım:
SELECT * FROM sp_performans_raporu();

-- Dönüş Kolonları:
-- FutbolcuAd, ToplamAntrenman, KatilimSayisi, 
-- OrtalamaPerformans, KatilimYuzdesi
```

#### `sp_sozlesme_biten_futbolcular`
**Parametre:** ay_sayisi (varsayılan: 6)  
**Dönüş:** Sözleşmesi bitmek üzere olan futbolcular

```sql
-- 6 ay içinde bitenler:
SELECT * FROM sp_sozlesme_biten_futbolcular(6);

-- 3 ay içinde bitenler:
SELECT * FROM sp_sozlesme_biten_futbolcular(3);

-- Dönüş Kolonları:
-- FutbolcuAd, SozlesmeBitis, KalanGun
```

---

## 🔍 Tüm Stored Procedures Listesi

### PostgreSQL'de Görüntüle:
```sql
-- Tüm fonksiyonlar
\df

-- Sadece sp_ ile başlayanlar
SELECT proname, pg_get_functiondef(oid) 
FROM pg_proc 
WHERE proname LIKE 'sp_%';
```

---

## 🎯 Kurulum

### Tüm SP'leri Yükle:
```bash
cd StoredProcedures
for file in *.sql; do
    psql -U postgres -d futbol_takimi_db -f "$file"
done
```

### Windows PowerShell:
```powershell
cd StoredProcedures
Get-ChildItem *.sql | ForEach-Object {
    psql -U postgres -d futbol_takimi_db -f $_.Name
}
```

---

## 📊 Stored Procedure Özeti

| Kategori | Adet | Dosyalar |
|----------|------|----------|
| **Giriş** | 2 | sp_yonetici_giris, sp_antrenor_giris |
| **Futbolcu CRUD** | 4 | ekle, guncelle, sil, listele |
| **Antrenman** | 2 | sp_antrenman_ekle, sp_katilim_ekle |
| **Raporlama** | 2 | performans, sozlesme |
| **TOPLAM** | **10** | |

---

## 🧪 Test Senaryosu

```sql
-- 1. Giriş testi
SELECT * FROM sp_yonetici_giris('semih', 'semih123');

-- 2. Futbolcu ekleme
SELECT sp_futbolcu_ekle(
    'Test', 'Oyuncu', '2000-01-01',
    180, 75, 'Forvet', 99,
    30000, '2024-01-01', '2025-12-31',
    'Türkiye', 'Aktif', 'Test futbolcu'
);

-- 3. Futbolcu listeleme
SELECT * FROM sp_futbolcu_listele('Test');

-- 4. Performans raporu
SELECT * FROM sp_performans_raporu();

-- 5. Sözleşme kontrolü
SELECT * FROM sp_sozlesme_biten_futbolcular(6);
```

---

**Not:** Tüm stored procedures PostgreSQL 12+ uyumludur.
