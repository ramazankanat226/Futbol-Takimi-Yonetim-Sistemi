# Veritabanı Kurulum Rehberi

## 🎯 Hızlı Kurulum (Önerilen)

### Adım 1: PostgreSQL'i Başlatın
```bash
# PostgreSQL servisinin çalıştığından emin olun
# Windows: Services.msc > PostgreSQL
```

### Adım 2: Tek Komutla Kurulum
```bash
# Terminal veya CMD'de
cd Database\Schema
psql -U postgres -d postgres -f 00_TumKurulum_Full.sql
```

**VEYA** pgAdmin ile:
1. pgAdmin'i açın
2. `00_TumKurulum_Full.sql` dosyasını açın
3. F5'e basarak çalıştırın

### Adım 3: Kontrol
```sql
-- Tabloları kontrol et
SELECT * FROM Yoneticiler;
SELECT * FROM Futbolcular;

-- SP'leri kontrol et
SELECT routine_name FROM information_schema.routines 
WHERE routine_schema = 'public' AND routine_name LIKE 'sp_%';

-- Trigger'ları kontrol et
SELECT trigger_name FROM information_schema.triggers;
```

---

## 📋 Detaylı Kurulum

### 1. Veritabanı Oluşturma
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

### 2. Tablolar
```bash
# Schema klasöründe sırayla çalıştırın
psql -U postgres -d futbol_takimi_db -f 01_Tables.sql
psql -U postgres -d futbol_takimi_db -f 02_Indexes.sql
psql -U postgres -d futbol_takimi_db -f 03_Constraints.sql
psql -U postgres -d futbol_takimi_db -f 04_InitialData.sql
```

### 3. Stored Procedures
```bash
# StoredProcedures klasöründe
cd ..\StoredProcedures
psql -U postgres -d futbol_takimi_db -f sp_antrenman_crud.sql
psql -U postgres -d futbol_takimi_db -f sp_futbolcu_antrenman_crud.sql
```

### 4. Triggers
```bash
# Triggers klasöründe
cd ..\Triggers
psql -U postgres -d futbol_takimi_db -f trg_futbolcu_insert.sql
psql -U postgres -d futbol_takimi_db -f trg_futbolcu_log.sql
psql -U postgres -d futbol_takimi_db -f trg_antrenman_log.sql
```

---

## 🔧 Sorun Giderme

### Sorun: Veritabanı zaten mevcut
```sql
-- Tüm veritabanını sil ve yeniden oluştur
DROP DATABASE IF EXISTS futbol_takimi_db;
-- Sonra 00_TumKurulum_Full.sql çalıştır
```

### Sorun: Stored Procedure hatası
```sql
-- Tüm SP'leri temizle
DROP FUNCTION IF EXISTS sp_futbolcu_ekle CASCADE;
-- Sonra StoredProcedures klasöründeki dosyaları çalıştır
```

### Sorun: Trigger çalışmıyor
```sql
-- Trigger'ları kontrol et
SELECT * FROM pg_trigger WHERE tgname LIKE 'trg_%';

-- Trigger'ı yeniden oluştur
DROP TRIGGER IF EXISTS trg_futbolcu_insert ON Futbolcular;
-- Sonra trigger dosyasını çalıştır
```

### Sorun: Test verileri yok
```bash
# TestData klasöründe
cd Database\TestData
psql -U postgres -d futbol_takimi_db -f test_kullanicilar.sql
```

---

## ✅ Kontrol Listesi

### Tablolar (7 adet)
- [ ] Yoneticiler
- [ ] Antrenorler
- [ ] Futbolcular
- [ ] Antrenmanlar
- [ ] FutbolcuAntrenman
- [ ] LogTablosu
- [ ] Bildirimler

### Stored Procedures (17 adet)
```sql
-- Bu sorgu ile kontrol edin
SELECT routine_name 
FROM information_schema.routines 
WHERE routine_schema = 'public' AND routine_name LIKE 'sp_%'
ORDER BY routine_name;

-- Beklenen sonuç:
-- sp_antrenman_ekle
-- sp_antrenman_guncelle
-- sp_antrenman_katilimci_listele
-- sp_antrenman_listele
-- sp_antrenman_sil
-- sp_antrenor_giris
-- sp_futbolcu_antrenman_guncelle
-- sp_futbolcu_antrenman_listele
-- sp_futbolcu_antrenman_sil
-- sp_futbolcu_ekle
-- sp_futbolcu_guncelle
-- sp_futbolcu_listele
-- sp_futbolcu_sil
-- sp_katilim_ekle
-- sp_performans_raporu
-- sp_sozlesme_biten_futbolcular
-- sp_yonetici_giris
```

### Triggers (8 adet)
```sql
-- Bu sorgu ile kontrol edin
SELECT trigger_name, event_object_table, action_timing, event_manipulation
FROM information_schema.triggers
WHERE trigger_schema = 'public'
ORDER BY trigger_name;

-- Beklenen sonuçlar:
-- trg_antrenman_delete (Antrenmanlar, AFTER DELETE)
-- trg_antrenman_insert (Antrenmanlar, AFTER INSERT)
-- trg_antrenman_update (Antrenmanlar, AFTER UPDATE)
-- trg_futbolcu_delete (Futbolcular, AFTER DELETE)
-- trg_futbolcu_insert (Futbolcular, AFTER INSERT)
-- trg_futbolcu_update (Futbolcular, AFTER UPDATE)
-- trg_sozlesme_uyari (Futbolcular, AFTER INSERT OR UPDATE)
-- trg_update_timestamp (Yoneticiler/Antrenorler, BEFORE UPDATE)
```

### Test Verileri
```sql
-- Kontrol sorguları
SELECT COUNT(*) FROM Yoneticiler;      -- Beklenen: 2
SELECT COUNT(*) FROM Antrenorler;      -- Beklenen: 3
SELECT COUNT(*) FROM Futbolcular;      -- Beklenen: 10
SELECT COUNT(*) FROM Antrenmanlar;     -- Beklenen: 6
SELECT COUNT(*) FROM FutbolcuAntrenman;-- Beklenen: 8+
```

### Test Kullanıcıları
- [ ] admin / admin123 (Yönetici)
- [ ] teknikdirektor / antrenor123 (Antrenör)

---

## 📊 Veritabanı İstatistikleri

```sql
-- Tüm istatistikler
SELECT 
    (SELECT COUNT(*) FROM Yoneticiler) as yonetici_sayisi,
    (SELECT COUNT(*) FROM Antrenorler) as antrenor_sayisi,
    (SELECT COUNT(*) FROM Futbolcular) as futbolcu_sayisi,
    (SELECT COUNT(*) FROM Antrenmanlar) as antrenman_sayisi,
    (SELECT COUNT(*) FROM FutbolcuAntrenman) as katilim_sayisi,
    (SELECT COUNT(*) FROM LogTablosu) as log_sayisi;
```

---

## 🔄 Veritabanını Sıfırlama

```sql
-- TEHLİKELİ: Tüm verileri siler!
DROP DATABASE IF EXISTS futbol_takimi_db;

-- Yeniden oluştur
-- 00_TumKurulum_Full.sql dosyasını çalıştır
```

---

## 📝 Notlar

1. **00_TumKurulum_Full.sql** tek dosyada tüm kurulumu yapar (önerilen)
2. **Schema** klasöründeki dosyalar parça parça kurulum için
3. **StoredProcedures** ve **Triggers** klasörleri ek SP'ler için
4. **TestData** klasörü test kullanıcıları için
5. Tüm SP'ler ve trigger'lar `00_TumKurulum_Full.sql` içinde zaten var

---

## 🆘 Yardım

Sorun yaşıyorsanız:
1. `TEST_STORED_PROCEDURES.sql` dosyasını çalıştırın
2. Hata mesajlarını kontrol edin
3. pgAdmin'de log'lara bakın
4. PostgreSQL versiyonunu kontrol edin (12+ olmalı)

---

**Son Güncelleme:** 23 Aralık 2024


