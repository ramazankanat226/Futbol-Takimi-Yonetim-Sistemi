# Futbol Takımı Yönetim Sistemi Veritabanı Şeması

Bu belge, Access veritabanı için önerilen tablo yapılarını içerir.

## Tablolar

### 1. Yoneticiler
- **YoneticiID** (AutoNumber, Primary Key)
- **KullaniciAdi** (Text)
- **Sifre** (Text)
- **Ad** (Text)
- **Soyad** (Text)
- **Eposta** (Text)
- **TelefonNo** (Text)

### 2. Futbolcular
- **FutbolcuID** (AutoNumber, Primary Key)
- **Ad** (Text)
- **Soyad** (Text)
- **DogumTarihi** (Date/Time)
- **Boy** (Number - cm)
- **Kilo** (Number - kg)
- **Pozisyon** (Text)
- **FormaNo** (Number)
- **Maas** (Currency)
- **SozlesmeBaslangic** (Date/Time)
- **SozlesmeBitis** (Date/Time)
- **Uyruk** (Text)
- **Durumu** (Text - Aktif/Sakat/Cezalı)
- **Resim** (OLE Object - opsiyonel)

### 3. Antrenmanlar
- **AntrenmanID** (AutoNumber, Primary Key)
- **Tarih** (Date/Time)
- **BaslangicSaati** (Date/Time)
- **BitisSaati** (Date/Time)
- **Tur** (Text - Kondisyon/Taktik/Teknik)
- **Notlar** (Memo)

### 4. FutbolcuAntrenman
- **FutbolcuAntrenmanID** (AutoNumber, Primary Key)
- **FutbolcuID** (Number, Foreign Key)
- **AntrenmanID** (Number, Foreign Key)
- **Katilim** (Yes/No)
- **Performans** (Number - 1-10)
- **Notlar** (Memo)

## İlişkiler
- **Futbolcular** ile **FutbolcuAntrenman** arasında one-to-many ilişki
- **Antrenmanlar** ile **FutbolcuAntrenman** arasında one-to-many ilişki

## Örnek Veri
Yöneticiler tablosuna en azından bir admin kullanıcısı ekleyin:
- KullaniciAdi: admin
- Sifre: admin123

Not: Gerçek uygulamada güvenli şifre politikaları uygulanmalıdır. 