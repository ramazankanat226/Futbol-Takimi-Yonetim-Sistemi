# ⚡ TRIGGERLAR

Veritabanında otomatik çalışan 5 adet trigger bulunmaktadır.

## 📋 Trigger Listesi

### 1️⃣ Audit Log Triggerları (3 adet)

#### `trg_futbolcu_insert.sql`
**Tetiklenme:** Yeni futbolcu eklendiğinde  
**Amaç:** INSERT işlemini LogTablosu'na kaydeder  
**Kayıt:** Yeni eklenen futbolcu bilgileri JSON formatında

```sql
-- Örnek log kaydı:
-- Islem: INSERT
-- YeniVeri: {"ad": "Ahmet", "soyad": "Yılmaz", ...}
```

#### `trg_futbolcu_update.sql`
**Tetiklenme:** Futbolcu bilgisi güncellendiğinde  
**Amaç:** UPDATE işlemini LogTablosu'na kaydeder  
**Kayıt:** Hem eski hem yeni değerler JSON formatında

```sql
-- Örnek log kaydı:
-- Islem: UPDATE
-- EskiVeri: {"maas": 50000}
-- YeniVeri: {"maas": 75000}
```

#### `trg_futbolcu_delete.sql`
**Tetiklenme:** Futbolcu silindiğinde  
**Amaç:** DELETE işlemini LogTablosu'na kaydeder  
**Kayıt:** Silinen futbolcu bilgileri JSON formatında

```sql
-- Örnek log kaydı:
-- Islem: DELETE
-- EskiVeri: {"futbolcuid": 1, "ad": "Ahmet", ...}
```

---

### 2️⃣ İş Kuralı Triggerları (2 adet)

#### `trg_sozlesme_uyari.sql`
**Tetiklenme:** Futbolcu eklendiğinde veya sözleşme güncellendiğinde  
**Amaç:** Sözleşme bitiş tarihine 6 ay kala otomatik bildirim oluşturur  
**Kontrol:** SozlesmeBitis <= CURRENT_DATE + 6 months

```sql
-- Örnek bildirim:
-- BaslikTipi: 'SozlesmeBitiyor'
-- Mesaj: 'DİKKAT: Ahmet Yılmaz isimli futbolcunun sözleşmesi ... tarihinde bitiyor!'
```

---

## 🔍 Trigger Kontrolü

### Mevcut Triggerları Görüntüle:
```sql
SELECT 
    tgname AS trigger_adi,
    tgrelid::regclass AS tablo_adi,
    proname AS fonksiyon_adi
FROM pg_trigger 
JOIN pg_proc ON pg_trigger.tgfoid = pg_proc.oid
WHERE tgisinternal = FALSE
ORDER BY tgname;
```

### Log Kayıtlarını Görüntüle:
```sql
-- Son 10 işlem
SELECT * FROM LogTablosu 
ORDER BY IslemZamani DESC 
LIMIT 10;

-- Belirli bir tablonun logları
SELECT * FROM LogTablosu 
WHERE TabloAdi = 'Futbolcular'
ORDER BY IslemZamani DESC;
```

### Bildirimleri Görüntüle:
```sql
-- Okunmamış bildirimler
SELECT * FROM Bildirimler 
WHERE Okundu = FALSE;

-- Sözleşme uyarıları
SELECT * FROM Bildirimler 
WHERE BaslikTipi = 'SozlesmeBitiyor';
```

---

## 🎯 Kurulum

### Tüm Triggerları Yükle:
```bash
psql -U postgres -d futbol_takimi_db -f trg_futbolcu_insert.sql
psql -U postgres -d futbol_takimi_db -f trg_futbolcu_update.sql
psql -U postgres -d futbol_takimi_db -f trg_futbolcu_delete.sql
psql -U postgres -d futbol_takimi_db -f trg_sozlesme_uyari.sql
```

### Tek Tek Yükle:
```bash
cd Triggers
psql -U postgres -d futbol_takimi_db -f [trigger_dosyasi].sql
```

---

## 📊 Trigger İstatistikleri

| Trigger | Tablo | Olay | Kayıt Yeri |
|---------|-------|------|------------|
| trg_futbolcu_insert | Futbolcular | INSERT | LogTablosu |
| trg_futbolcu_update | Futbolcular | UPDATE | LogTablosu |
| trg_futbolcu_delete | Futbolcular | DELETE | LogTablosu |
| trg_sozlesme_uyari | Futbolcular | INSERT/UPDATE | Bildirimler |

---

**Not:** Triggerlar otomatik çalışır, manuel müdahale gerekmez.
