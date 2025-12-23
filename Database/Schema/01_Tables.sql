-- ============================================
-- Futbol Takımı Yönetim Sistemi - PostgreSQL
-- Tablo Şemaları
-- ============================================

-- Veritabanı oluşturma (psql ile çalıştırılmalı)
-- CREATE DATABASE futbol_takimi_db
--     WITH 
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'Turkish_Turkey.1254'
--     LC_CTYPE = 'Turkish_Turkey.1254'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1;

-- Veritabanına bağlan
\c futbol_takimi_db;

-- ============================================
-- 1. Yoneticiler Tablosu
-- ============================================
DROP TABLE IF EXISTS Yoneticiler CASCADE;

CREATE TABLE Yoneticiler (
    YoneticiID SERIAL PRIMARY KEY,
    KullaniciAdi VARCHAR(50) UNIQUE NOT NULL,
    SifreHash VARCHAR(255) NOT NULL,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    Eposta VARCHAR(100) UNIQUE,
    TelefonNo VARCHAR(20),
    KullaniciTipi VARCHAR(20) DEFAULT 'Yonetici' CHECK (KullaniciTipi IN ('Admin', 'Yonetici')),
    Aktif BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

COMMENT ON TABLE Yoneticiler IS 'Sistem yöneticileri ve yönetici kullanıcılar';
COMMENT ON COLUMN Yoneticiler.SifreHash IS 'BCrypt ile hashlenmis sifre';
COMMENT ON COLUMN Yoneticiler.KullaniciTipi IS 'Admin: Tam yetki, Yonetici: Kisitli yetki';

-- ============================================
-- 2. Antrenorler Tablosu (YENİ - 2. Kullanıcı Tipi)
-- ============================================
DROP TABLE IF EXISTS Antrenorler CASCADE;

CREATE TABLE Antrenorler (
    AntrenorID SERIAL PRIMARY KEY,
    KullaniciAdi VARCHAR(50) UNIQUE NOT NULL,
    SifreHash VARCHAR(255) NOT NULL,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    Eposta VARCHAR(100) UNIQUE,
    TelefonNo VARCHAR(20),
    Uzmanlik VARCHAR(50) CHECK (Uzmanlik IN ('Kondisyon', 'Teknik', 'Taktik', 'Genel')),
    KullaniciTipi VARCHAR(20) DEFAULT 'Antrenor' CHECK (KullaniciTipi IN ('Antrenor', 'TeknikDirektor', 'YardimciAntrenor')),
    Aktif BOOLEAN DEFAULT TRUE,
    IseBaslamaTarihi DATE DEFAULT CURRENT_DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

COMMENT ON TABLE Antrenorler IS 'Antrenörler ve teknik direktör - 2. kullanıcı tipi';
COMMENT ON COLUMN Antrenorler.Uzmanlik IS 'Antrenörün uzmanlık alanı';
COMMENT ON COLUMN Antrenorler.KullaniciTipi IS 'TeknikDirektor: Tüm antrenman yetkileri';

-- ============================================
-- 3. Futbolcular Tablosu
-- ============================================
DROP TABLE IF EXISTS Futbolcular CASCADE;

CREATE TABLE Futbolcular (
    FutbolcuID SERIAL PRIMARY KEY,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    DogumTarihi DATE NOT NULL,
    Boy INTEGER CHECK (Boy > 0 AND Boy < 250),
    Kilo INTEGER CHECK (Kilo > 0 AND Kilo < 200),
    Pozisyon VARCHAR(30) NOT NULL CHECK (Pozisyon IN (
        'Kaleci', 'Defans', 'Orta Saha', 'Forvet', 
        'Sağ Bek', 'Sol Bek', 'Stoper', 'Defansif Orta Saha',
        'Ofansif Orta Saha', 'Kanat', 'Santrafor'
    )),
    FormaNo INTEGER UNIQUE CHECK (FormaNo > 0 AND FormaNo <= 99),
    Maas DECIMAL(12, 2) CHECK (Maas >= 0),
    SozlesmeBaslangic DATE NOT NULL,
    SozlesmeBitis DATE NOT NULL,
    Uyruk VARCHAR(50) NOT NULL,
    Durumu VARCHAR(20) DEFAULT 'Aktif' CHECK (Durumu IN ('Aktif', 'Sakat', 'Cezalı', 'Pasif')),
    Resim BYTEA,
    Notlar TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT chk_sozlesme_tarih CHECK (SozlesmeBaslangic < SozlesmeBitis)
);

COMMENT ON TABLE Futbolcular IS 'Futbolcu bilgileri ve sözleşme detayları';
COMMENT ON COLUMN Futbolcular.Durumu IS 'Futbolcunun aktif durumu';
COMMENT ON COLUMN Futbolcular.FormaNo IS 'Benzersiz forma numarası';

-- ============================================
-- 4. Antrenmanlar Tablosu
-- ============================================
DROP TABLE IF EXISTS Antrenmanlar CASCADE;

CREATE TABLE Antrenmanlar (
    AntrenmanID SERIAL PRIMARY KEY,
    Tarih DATE NOT NULL,
    BaslangicSaati TIME NOT NULL,
    BitisSaati TIME NOT NULL,
    Tur VARCHAR(30) NOT NULL CHECK (Tur IN ('Kondisyon', 'Taktik', 'Teknik', 'Hazırlık Maçı', 'Toparlanma')),
    Notlar TEXT,
    AntrenorID INTEGER NOT NULL,
    Lokasyon VARCHAR(100) DEFAULT 'Ana Tesis',
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_antrenman_antrenor FOREIGN KEY (AntrenorID) 
        REFERENCES Antrenorler(AntrenorID) ON DELETE RESTRICT,
    CONSTRAINT chk_antrenman_saat CHECK (BaslangicSaati < BitisSaati)
);

COMMENT ON TABLE Antrenmanlar IS 'Antrenman programı ve detayları';
COMMENT ON COLUMN Antrenmanlar.AntrenorID IS 'Antrenmanı yöneten antrenör';
COMMENT ON COLUMN Antrenmanlar.Tur IS 'Antrenman türü';

-- ============================================
-- 5. FutbolcuAntrenman Tablosu (İlişki Tablosu)
-- ============================================
DROP TABLE IF EXISTS FutbolcuAntrenman CASCADE;

CREATE TABLE FutbolcuAntrenman (
    FutbolcuAntrenmanID SERIAL PRIMARY KEY,
    FutbolcuID INTEGER NOT NULL,
    AntrenmanID INTEGER NOT NULL,
    Katilim BOOLEAN DEFAULT FALSE,
    Performans INTEGER CHECK (Performans >= 1 AND Performans <= 10),
    Notlar TEXT,
    KayitZamani TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_futbolcu_antrenman_futbolcu FOREIGN KEY (FutbolcuID) 
        REFERENCES Futbolcular(FutbolcuID) ON DELETE CASCADE,
    CONSTRAINT fk_futbolcu_antrenman_antrenman FOREIGN KEY (AntrenmanID) 
        REFERENCES Antrenmanlar(AntrenmanID) ON DELETE CASCADE,
    CONSTRAINT uq_futbolcu_antrenman UNIQUE (FutbolcuID, AntrenmanID)
);

COMMENT ON TABLE FutbolcuAntrenman IS 'Futbolcuların antrenmanlara katılım ve performans kayıtları';
COMMENT ON COLUMN FutbolcuAntrenman.Performans IS '1-10 arası performans değerlendirmesi';
COMMENT ON CONSTRAINT uq_futbolcu_antrenman ON FutbolcuAntrenman IS 'Bir futbolcu aynı antrenmana sadece 1 kez eklenebilir';

-- ============================================
-- 6. LogTablosu (Audit Trail - İzleme)
-- ============================================
DROP TABLE IF EXISTS LogTablosu CASCADE;

CREATE TABLE LogTablosu (
    LogID SERIAL PRIMARY KEY,
    TabloAdi VARCHAR(50) NOT NULL,
    Islem VARCHAR(20) NOT NULL CHECK (Islem IN ('INSERT', 'UPDATE', 'DELETE')),
    KullaniciID INTEGER,
    KullaniciTipi VARCHAR(20) CHECK (KullaniciTipi IN ('Yonetici', 'Antrenor', 'Admin', 'Sistem')),
    IslemZamani TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    EskiVeri JSONB,
    YeniVeri JSONB,
    IPAdresi VARCHAR(50),
    Aciklama TEXT
);

COMMENT ON TABLE LogTablosu IS 'Tüm veritabanı işlemlerinin audit logu';
COMMENT ON COLUMN LogTablosu.EskiVeri IS 'UPDATE ve DELETE işlemlerinde eski veri (JSON)';
COMMENT ON COLUMN LogTablosu.YeniVeri IS 'INSERT ve UPDATE işlemlerinde yeni veri (JSON)';

-- ============================================
-- 7. Bildirimler Tablosu (Sözleşme Uyarıları vs.)
-- ============================================
DROP TABLE IF EXISTS Bildirimler CASCADE;

CREATE TABLE Bildirimler (
    BildirimID SERIAL PRIMARY KEY,
    BaslikTipi VARCHAR(50) NOT NULL CHECK (BaslikTipi IN ('SozlesmeBitiyor', 'SakatlikUyarisi', 'CezaSuresi', 'Genel')),
    FutbolcuID INTEGER,
    Mesaj TEXT NOT NULL,
    Okundu BOOLEAN DEFAULT FALSE,
    OlusturmaTarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_bildirim_futbolcu FOREIGN KEY (FutbolcuID) 
        REFERENCES Futbolcular(FutbolcuID) ON DELETE CASCADE
);

COMMENT ON TABLE Bildirimler IS 'Sistem bildirimleri ve uyarılar';
COMMENT ON COLUMN Bildirimler.BaslikTipi IS 'Bildirim türü';

-- ============================================
-- İndeks tanımlamaları 02_Indexes.sql dosyasında
-- Constraint'ler 03_Constraints.sql dosyasında
-- Başlangıç verileri 04_InitialData.sql dosyasında
-- ============================================

-- Başarıyla tamamlandı mesajı
DO $$
BEGIN
    RAISE NOTICE 'Tablolar başarıyla oluşturuldu!';
    RAISE NOTICE 'Toplam 7 tablo: Yoneticiler, Antrenorler, Futbolcular, Antrenmanlar, FutbolcuAntrenman, LogTablosu, Bildirimler';
END $$;

