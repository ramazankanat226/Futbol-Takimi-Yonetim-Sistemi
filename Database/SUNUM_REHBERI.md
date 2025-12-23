# 📊 SUNUM REHBERİ - Veritabanı Bileşenleri

## 🎯 Hızlı Erişim

| Bileşen | Dosya | Açıklama |
|---------|-------|----------|
| **Tablolar** | `Schema/01_Tables.sql` | 7 tablo tanımı |
| **İndeksler** | `Schema/02_Indexes.sql` | Performans optimizasyonu |
| **Kısıtlar** | `Schema/03_Constraints.sql` | Foreign Key ve Check |
| **Başlangıç Verileri** | `Schema/04_InitialData.sql` | Test verileri |
| **Stored Procedures** | `StoredProcedures/` klasörü | 10 adet |
| **Triggerlar** | `Triggers/` klasörü | 5 adet |

---

## 📋 1. TABLOLAR (Schema/01_Tables.sql)

### 7 Ana Tablo:
1. **Yoneticiler** - Sistem yöneticileri (Admin, Yönetici)
2. **Antrenorler** - Antrenörler (TeknikDirektor, Antrenör)
3. **Futbolcular** - Futbolcu bilgileri ve sözleşmeler
4. **Antrenmanlar** - Antrenman programı
5. **FutbolcuAntrenman** - Katılım ve performans (Many-to-Many)
6. **LogTablosu** - Audit trail (INSERT/UPDATE/DELETE kayıtları)
7. **Bildirimler** - Sistem bildirimleri (sözleşme, sakatlık vs.)

### Önemli Özellikler:
- ✅ BCrypt şifre hash desteği
- ✅ Cascading delete
- ✅ Check constraints (yaş, boy, kilo vb.)
- ✅ Timestamp otomasyonu

---

## 🔐 2. STORED PROCEDURES (StoredProcedures/)

### Giriş ve Yetkilendirme:
- `sp_yonetici_giris.sql` - Yönetici giriş kontrolü
- `sp_antrenor_giris.sql` - Antrenör giriş kontrolü

### Futbolcu CRUD:
- `sp_futbolcu_ekle.sql` - Yeni futbolcu kaydı
- `sp_futbolcu_guncelle.sql` - Futbolcu bilgisi güncelleme
- `sp_futbolcu_sil.sql` - Futbolcu silme (CASCADE)
- `sp_futbolcu_listele.sql` - Futbolcu listesi ve arama

### Antrenman Yönetimi:
- `sp_antrenman_ekle.sql` - Yeni antrenman oluşturma
- `sp_katilim_ekle.sql` - Futbolcu katılım kaydı

### Raporlama:
- `sp_performans_raporu.sql` - Futbolcu performans analizi
- `sp_sozlesme_biten_futbolcular.sql` - Sözleşme uyarı raporu

---

## ⚡ 3. TRIGGERLAR (Triggers/)

### Audit Log Triggerları:
1. **trg_futbolcu_insert.sql**
   - Yeni futbolcu eklendiğinde log kaydı
   - LogTablosu'na INSERT işlemi

2. **trg_futbolcu_update.sql**
   - Futbolcu güncellendiğinde log kaydı
   - Eski ve yeni değerleri JSON formatında saklar

3. **trg_futbolcu_delete.sql**
   - Futbolcu silindiğinde log kaydı
   - Silinen veriyi JSON olarak saklar

### İş Kuralı Triggerları:
4. **trg_sozlesme_uyari.sql**
   - Sözleşme bitiş tarihi yaklaşınca bildirim
   - 6 ay kala otomatik uyarı oluşturur

---

## 🗂️ 4. KURULUM DOSYALARI

### Tam Kurulum:
```bash
psql -U postgres -f Schema/00_TumKurulum_Full.sql
```
**İçerik:**
- Veritabanı oluşturma
- Tüm tablolar
- İndeksler ve kısıtlar
- Stored procedures
- Triggerlar
- Örnek veriler

### Adım Adım Kurulum:
```bash
psql -U postgres -f Schema/01_Tables.sql
psql -U postgres -f Schema/02_Indexes.sql
psql -U postgres -f Schema/03_Constraints.sql
psql -U postgres -f Schema/04_InitialData.sql
```

---

## 👥 5. KULLANICI SİSTEMİ

### Test Kullanıcıları:
| Tip | Kullanıcı Adı | Şifre | Yetki |
|-----|---------------|-------|-------|
| Yönetici | semih | semih123 | CRUD + Raporlar |
| Antrenör | ramazan | ramazan123 | Sadece Görüntüleme |

### Yetki Matrisi:
| İşlem | Yönetici | Antrenör |
|-------|----------|----------|
| Futbolcu Görüntüle | ✅ | ✅ |
| Futbolcu Ekle | ✅ | ❌ |
| Futbolcu Düzenle | ✅ | ❌ |
| Futbolcu Sil | ✅ | ❌ |
| Antrenman Görüntüle | ✅ | ✅ |
| Antrenman Ekle | ✅ | ✅ |
| Katılım Kaydet | ✅ | ✅ |

---

## 📊 6. VERİTABANI ŞEMASI

```
┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│ Yoneticiler │         │  Antrenorler │         │ Futbolcular │
└─────────────┘         └──────────────┘         └─────────────┘
                               │                        │
                               │                        │
                               ▼                        │
                        ┌──────────────┐               │
                        │ Antrenmanlar │◄──────────────┘
                        └──────────────┘      Many-to-Many
                               │
                               ▼
                    ┌──────────────────────┐
                    │ FutbolcuAntrenman   │
                    │ (İlişki Tablosu)    │
                    └──────────────────────┘

        ┌────────────┐                  ┌──────────────┐
        │ LogTablosu │                  │ Bildirimler  │
        │ (Audit)    │                  │ (Uyarılar)   │
        └────────────┘                  └──────────────┘
```

---

## 🎓 7. SUNUMDA SÖYLENEBİLECEKLER

### Veritabanı Tasarımı:
- ✅ **Normalleştirilmiş yapı** - 3NF uyumlu
- ✅ **İlişkisel bütünlük** - Foreign Key constraints
- ✅ **Veri doğrulama** - Check constraints
- ✅ **Audit trail** - LogTablosu ile tüm işlemler kayıtlı
- ✅ **Cascade işlemler** - Veri tutarlılığı

### Güvenlik:
- ✅ **BCrypt şifreleme** - Güvenli parola saklama
- ✅ **Rol bazlı yetkilendirme** - Yönetici/Antrenör
- ✅ **Session yönetimi** - Singleton pattern

### Performans:
- ✅ **İndeksler** - Hızlı arama ve sorgulama
- ✅ **Stored procedures** - Sunucu tarafı işlem
- ✅ **Triggerlar** - Otomatik iş kuralları

### Kullanılabilirlik:
- ✅ **Modern UI** - WinForms + özel stiller
- ✅ **Responsive tasarım** - Tam ekran desteği
- ✅ **Kullanıcı dostu** - Kolay navigasyon

---

## 📝 8. DEMO SENARYOSU

### 1. Veritabanı Gösterimi (5 dk)
```sql
-- Tabloları göster
\dt

-- Triggerları göster
SELECT tgname FROM pg_trigger;

-- Stored procedures'ları göster
\df
```

### 2. Uygulama Gösterimi (10 dk)
1. **Giriş Ekranı** → Yönetici/Antrenör seçimi
2. **Yönetici Girişi** → Tüm yetkiler
3. **Futbolcu Listesi** → CRUD işlemleri
4. **Antrenör Girişi** → Sadece görüntüleme
5. **Yetki Kontrolü** → Butonlar devre dışı

### 3. Veritabanı Kontrolü (5 dk)
```sql
-- Log kayıtları
SELECT * FROM LogTablosu ORDER BY IslemZamani DESC LIMIT 10;

-- Bildirimler
SELECT * FROM Bildirimler WHERE Okundu = FALSE;

-- Performans raporu
SELECT * FROM sp_performans_raporu();
```

---

## 🚀 HIZLI TEST

### Veritabanı Bağlantı Testi:
```sql
SELECT * FROM Yoneticiler;
SELECT * FROM Antrenorler;
SELECT * FROM Futbolcular LIMIT 5;
```

### Stored Procedure Testi:
```sql
-- Giriş kontrolü
SELECT * FROM sp_yonetici_giris('semih', 'semih123');

-- Futbolcu arama
SELECT * FROM sp_futbolcu_listele('');
```

### Trigger Testi:
```sql
-- Yeni futbolcu ekle
INSERT INTO Futbolcular (...) VALUES (...);

-- Log tablosunu kontrol et
SELECT * FROM LogTablosu ORDER BY IslemZamani DESC LIMIT 1;
```

---

## 📚 DÖKÜMANTASYON

- **KURULUM_REHBERI.md** - Detaylı kurulum adımları
- **README_DATABASE.md** - Veritabanı dokümantasyonu
- **README.md** - Genel proje bilgileri

---

**Not:** Tüm dosyalar UTF-8 encoding ile kaydedilmiştir. PostgreSQL 12+ gerektirir.






