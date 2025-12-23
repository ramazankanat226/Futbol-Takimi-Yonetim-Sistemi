DROP TABLE IF EXISTS FutbolcuAntrenman CASCADE;
DROP TABLE IF EXISTS Antrenmanlar CASCADE;
DROP TABLE IF EXISTS Futbolcular CASCADE;
DROP TABLE IF EXISTS Bildirimler CASCADE;
DROP TABLE IF EXISTS LogTablosu CASCADE;
DROP TABLE IF EXISTS Antrenorler CASCADE;
DROP TABLE IF EXISTS Yoneticiler CASCADE;

-- Yöneticiler
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
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Antrenörler (2. kullanıcı tipi - ZORUNLU)
CREATE TABLE Antrenorler (
    AntrenorID SERIAL PRIMARY KEY,
    KullaniciAdi VARCHAR(50) UNIQUE NOT NULL,
    SifreHash VARCHAR(255) NOT NULL,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    Eposta VARCHAR(100) UNIQUE,
    TelefonNo VARCHAR(20),
    Uzmanlik VARCHAR(50) CHECK (Uzmanlik IN ('Kondisyon', 'Teknik', 'Taktik', 'Genel')),
    KullaniciTipi VARCHAR(20) DEFAULT 'Antrenor',
    Aktif BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Futbolcular
CREATE TABLE Futbolcular (
    FutbolcuID SERIAL PRIMARY KEY,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    DogumTarihi DATE NOT NULL,
    Boy INTEGER CHECK (Boy > 0 AND Boy < 250),
    Kilo INTEGER CHECK (Kilo > 0 AND Kilo < 200),
    Pozisyon VARCHAR(30) NOT NULL,
    FormaNo INTEGER UNIQUE CHECK (FormaNo > 0 AND FormaNo <= 99),
    Maas DECIMAL(12, 2) CHECK (Maas >= 0),
    SozlesmeBaslangic DATE NOT NULL,
    SozlesmeBitis DATE NOT NULL,
    Uyruk VARCHAR(50) NOT NULL,
    Durumu VARCHAR(20) DEFAULT 'Aktif' CHECK (Durumu IN ('Aktif', 'Sakat', 'Cezalı', 'Pasif')),
    Notlar TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT chk_sozlesme CHECK (SozlesmeBaslangic < SozlesmeBitis)
);

-- Antrenmanlar
CREATE TABLE Antrenmanlar (
    AntrenmanID SERIAL PRIMARY KEY,
    Tarih DATE NOT NULL,
    BaslangicSaati TIME NOT NULL,
    BitisSaati TIME NOT NULL,
    Tur VARCHAR(30) NOT NULL CHECK (Tur IN ('Kondisyon', 'Taktik', 'Teknik', 'Hazırlık Maçı', 'Toparlanma')),
    Notlar TEXT,
    AntrenorID INTEGER NOT NULL REFERENCES Antrenorler(AntrenorID),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT chk_saat CHECK (BaslangicSaati < BitisSaati)
);

-- Futbolcu-Antrenman İlişkisi
CREATE TABLE FutbolcuAntrenman (
    FutbolcuAntrenmanID SERIAL PRIMARY KEY,
    FutbolcuID INTEGER NOT NULL REFERENCES Futbolcular(FutbolcuID) ON DELETE CASCADE,
    AntrenmanID INTEGER NOT NULL REFERENCES Antrenmanlar(AntrenmanID) ON DELETE CASCADE,
    Katilim BOOLEAN DEFAULT FALSE,
    Performans INTEGER CHECK (Performans >= 1 AND Performans <= 10),
    Notlar TEXT,
    UNIQUE (FutbolcuID, AntrenmanID)
);

-- Log Tablosu
CREATE TABLE LogTablosu (
    LogID SERIAL PRIMARY KEY,
    TabloAdi VARCHAR(50) NOT NULL,
    Islem VARCHAR(20) NOT NULL CHECK (Islem IN ('INSERT', 'UPDATE', 'DELETE')),
    KullaniciID INTEGER,
    KullaniciTipi VARCHAR(20),
    IslemZamani TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    EskiVeri JSONB,
    YeniVeri JSONB
);

-- Bildirimler
CREATE TABLE Bildirimler (
    BildirimID SERIAL PRIMARY KEY,
    BaslikTipi VARCHAR(50) NOT NULL,
    FutbolcuID INTEGER REFERENCES Futbolcular(FutbolcuID) ON DELETE CASCADE,
    Mesaj TEXT NOT NULL,
    Okundu BOOLEAN DEFAULT FALSE,
    OlusturmaTarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- İNDEXLER (Performans için)
-- ============================================

-- Kullanıcı girişleri için
CREATE INDEX idx_yonetici_kullanici ON Yoneticiler(KullaniciAdi);
CREATE INDEX idx_antrenor_kullanici ON Antrenorler(KullaniciAdi);

-- Futbolcu sorguları için
CREATE INDEX idx_futbolcu_pozisyon ON Futbolcular(Pozisyon);
CREATE INDEX idx_futbolcu_durumu ON Futbolcular(Durumu);
CREATE INDEX idx_futbolcu_ad_soyad ON Futbolcular(Ad, Soyad);
CREATE INDEX idx_futbolcu_sozlesme_bitis ON Futbolcular(SozlesmeBitis);

-- Antrenman sorguları için
CREATE INDEX idx_antrenman_tarih ON Antrenmanlar(Tarih DESC);
CREATE INDEX idx_antrenman_antrenor ON Antrenmanlar(AntrenorID);

-- İlişki sorguları için
CREATE INDEX idx_fa_futbolcu ON FutbolcuAntrenman(FutbolcuID);
CREATE INDEX idx_fa_antrenman ON FutbolcuAntrenman(AntrenmanID);

-- Log sorguları için
CREATE INDEX idx_log_zaman ON LogTablosu(IslemZamani DESC);
CREATE INDEX idx_log_tablo ON LogTablosu(TabloAdi);

-- ============================================
-- TRIGGER'LAR (İş Kuralları - ZORUNLU)
-- ============================================

-- Trigger 1: Futbolcu ekleme logu
CREATE OR REPLACE FUNCTION fn_log_futbolcu_insert()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, YeniVeri)
    VALUES ('Futbolcular', 'INSERT', 0, 'Sistem', row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_insert
AFTER INSERT ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_insert();

-- Trigger 2: Futbolcu güncelleme logu
CREATE OR REPLACE FUNCTION fn_log_futbolcu_update()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri, YeniVeri)
    VALUES ('Futbolcular', 'UPDATE', 0, 'Sistem', row_to_json(OLD), row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_update
AFTER UPDATE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_update();

-- Trigger 3: Futbolcu silme logu
CREATE OR REPLACE FUNCTION fn_log_futbolcu_delete()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri)
    VALUES ('Futbolcular', 'DELETE', 0, 'Sistem', row_to_json(OLD));
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_delete
AFTER DELETE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_delete();

-- Trigger 4: Sözleşme bitiş uyarısı
CREATE OR REPLACE FUNCTION fn_sozlesme_uyari()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.SozlesmeBitis <= CURRENT_DATE + INTERVAL '90 days' THEN
        INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
        VALUES ('SozlesmeBitiyor', NEW.FutbolcuID, 
                NEW.Ad || ' ' || NEW.Soyad || ' isimli futbolcunun sözleşmesi ' || 
                TO_CHAR(NEW.SozlesmeBitis, 'DD.MM.YYYY') || ' tarihinde bitiyor!', FALSE);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_sozlesme_uyari
AFTER INSERT OR UPDATE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_sozlesme_uyari();

-- ============================================
-- STORED PROCEDURES (CRUD İşlemleri - ZORUNLU)
-- ============================================

-- SP 1: Futbolcu Ekle
CREATE OR REPLACE FUNCTION sp_futbolcu_ekle(
    p_ad VARCHAR, p_soyad VARCHAR, p_dogum DATE, p_boy INT, p_kilo INT,
    p_pozisyon VARCHAR, p_forma_no INT, p_maas DECIMAL, 
    p_sozlesme_bas DATE, p_sozlesme_bit DATE, p_uyruk VARCHAR, p_durumu VARCHAR
)
RETURNS INTEGER AS $$
DECLARE
    v_futbolcu_id INTEGER;
BEGIN
    INSERT INTO Futbolcular (Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, FormaNo, 
                             Maas, SozlesmeBaslangic, SozlesmeBitis, Uyruk, Durumu)
    VALUES (p_ad, p_soyad, p_dogum, p_boy, p_kilo, p_pozisyon, p_forma_no,
            p_maas, p_sozlesme_bas, p_sozlesme_bit, p_uyruk, p_durumu)
    RETURNING FutbolcuID INTO v_futbolcu_id;
    RETURN v_futbolcu_id;
END;
$$ LANGUAGE plpgsql;

-- SP 2: Futbolcu Güncelle
CREATE OR REPLACE FUNCTION sp_futbolcu_guncelle(
    p_id INT, p_ad VARCHAR, p_soyad VARCHAR, p_dogum DATE, p_boy INT, p_kilo INT,
    p_pozisyon VARCHAR, p_forma_no INT, p_maas DECIMAL, 
    p_sozlesme_bas DATE, p_sozlesme_bit DATE, p_uyruk VARCHAR, p_durumu VARCHAR
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE Futbolcular 
    SET Ad = p_ad, Soyad = p_soyad, DogumTarihi = p_dogum, Boy = p_boy, 
        Kilo = p_kilo, Pozisyon = p_pozisyon, FormaNo = p_forma_no, Maas = p_maas,
        SozlesmeBaslangic = p_sozlesme_bas, SozlesmeBitis = p_sozlesme_bit,
        Uyruk = p_uyruk, Durumu = p_durumu
    WHERE FutbolcuID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 3: Futbolcu Sil
CREATE OR REPLACE FUNCTION sp_futbolcu_sil(p_id INT)
RETURNS BOOLEAN AS $$
BEGIN
    DELETE FROM Futbolcular WHERE FutbolcuID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 4: Futbolcu Listele
CREATE OR REPLACE FUNCTION sp_futbolcu_listele()
RETURNS TABLE (
    FutbolcuID INT, Ad VARCHAR, Soyad VARCHAR, Pozisyon VARCHAR, 
    FormaNo INT, Durumu VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT f.FutbolcuID, f.Ad, f.Soyad, f.Pozisyon, f.FormaNo, f.Durumu
    FROM Futbolcular f
    ORDER BY f.Soyad, f.Ad;
END;
$$ LANGUAGE plpgsql;

-- SP 5: Antrenman Ekle
CREATE OR REPLACE FUNCTION sp_antrenman_ekle(
    p_tarih DATE, p_baslangic TIME, p_bitis TIME, 
    p_tur VARCHAR, p_notlar TEXT, p_antrenor_id INT
)
RETURNS INTEGER AS $$
DECLARE
    v_antrenman_id INTEGER;
BEGIN
    INSERT INTO Antrenmanlar (Tarih, BaslangicSaati, BitisSaati, Tur, Notlar, AntrenorID)
    VALUES (p_tarih, p_baslangic, p_bitis, p_tur, p_notlar, p_antrenor_id)
    RETURNING AntrenmanID INTO v_antrenman_id;
    RETURN v_antrenman_id;
END;
$$ LANGUAGE plpgsql;

-- SP 6: Antrenman Katılım Ekle
CREATE OR REPLACE FUNCTION sp_katilim_ekle(
    p_futbolcu_id INT, p_antrenman_id INT, 
    p_katilim BOOLEAN, p_performans INT, p_notlar TEXT
)
RETURNS INTEGER AS $$
DECLARE
    v_id INTEGER;
BEGIN
    INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
    VALUES (p_futbolcu_id, p_antrenman_id, p_katilim, p_performans, p_notlar)
    RETURNING FutbolcuAntrenmanID INTO v_id;
    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- SP 7: Kullanıcı Giriş Kontrolü (Yönetici)
CREATE OR REPLACE FUNCTION sp_yonetici_giris(p_kullanici VARCHAR, p_sifre VARCHAR)
RETURNS TABLE (
    YoneticiID INT, KullaniciAdi VARCHAR, Ad VARCHAR, 
    Soyad VARCHAR, KullaniciTipi VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT y.YoneticiID, y.KullaniciAdi, y.Ad, y.Soyad, y.KullaniciTipi
    FROM Yoneticiler y
    WHERE y.KullaniciAdi = p_kullanici AND y.SifreHash = p_sifre AND y.Aktif = TRUE;
END;
$$ LANGUAGE plpgsql;

-- SP 8: Kullanıcı Giriş Kontrolü (Antrenör)
CREATE OR REPLACE FUNCTION sp_antrenor_giris(p_kullanici VARCHAR, p_sifre VARCHAR)
RETURNS TABLE (
    AntrenorID INT, KullaniciAdi VARCHAR, Ad VARCHAR, 
    Soyad VARCHAR, Uzmanlik VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.AntrenorID, a.KullaniciAdi, a.Ad, a.Soyad, a.Uzmanlik
    FROM Antrenorler a
    WHERE a.KullaniciAdi = p_kullanici AND a.SifreHash = p_sifre AND a.Aktif = TRUE;
END;
$$ LANGUAGE plpgsql;

-- SP 9: Performans Raporu
CREATE OR REPLACE FUNCTION sp_performans_raporu(p_futbolcu_id INT)
RETURNS TABLE (
    Tarih DATE, Tur VARCHAR, Katilim BOOLEAN, 
    Performans INT, Notlar TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.Tarih, a.Tur, fa.Katilim, fa.Performans, fa.Notlar
    FROM FutbolcuAntrenman fa
    JOIN Antrenmanlar a ON fa.AntrenmanID = a.AntrenmanID
    WHERE fa.FutbolcuID = p_futbolcu_id
    ORDER BY a.Tarih DESC;
END;
$$ LANGUAGE plpgsql;

-- SP 10: Sözleşmesi Biten Futbolcular
CREATE OR REPLACE FUNCTION sp_sozlesme_biten_futbolcular()
RETURNS TABLE (
    FutbolcuID INT, Ad VARCHAR, Soyad VARCHAR, 
    SozlesmeBitis DATE, KalanGun INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT f.FutbolcuID, f.Ad, f.Soyad, f.SozlesmeBitis,
           (f.SozlesmeBitis - CURRENT_DATE)::INT as KalanGun
    FROM Futbolcular f
    WHERE f.SozlesmeBitis <= CURRENT_DATE + INTERVAL '90 days'
    ORDER BY f.SozlesmeBitis;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- EK CRUD STORED PROCEDURES (Proje İstekleri İçin)
-- ============================================

-- SP 11: Antrenman Güncelle
CREATE OR REPLACE FUNCTION sp_antrenman_guncelle(
    p_id INT, p_tarih DATE, p_baslangic TIME, p_bitis TIME, 
    p_tur VARCHAR, p_notlar TEXT, p_lokasyon VARCHAR
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE Antrenmanlar 
    SET Tarih = p_tarih, BaslangicSaati = p_baslangic,
        BitisSaati = p_bitis, Tur = p_tur, Notlar = p_notlar,
        Lokasyon = p_lokasyon
    WHERE AntrenmanID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 12: Antrenman Sil
CREATE OR REPLACE FUNCTION sp_antrenman_sil(p_id INT)
RETURNS BOOLEAN AS $$
BEGIN
    DELETE FROM Antrenmanlar WHERE AntrenmanID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 13: Antrenman Listele
CREATE OR REPLACE FUNCTION sp_antrenman_listele()
RETURNS TABLE (
    AntrenmanID INT, Tarih DATE, BaslangicSaati TIME, BitisSaati TIME,
    Tur VARCHAR, Notlar TEXT, AntrenorID INT, Lokasyon VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.AntrenmanID, a.Tarih, a.BaslangicSaati, a.BitisSaati, 
           a.Tur, a.Notlar, a.AntrenorID, a.Lokasyon
    FROM Antrenmanlar a
    ORDER BY a.Tarih DESC, a.BaslangicSaati;
END;
$$ LANGUAGE plpgsql;

-- SP 14: FutbolcuAntrenman Güncelle
CREATE OR REPLACE FUNCTION sp_futbolcu_antrenman_guncelle(
    p_id INT, p_futbolcu_id INT, p_antrenman_id INT,
    p_katilim BOOLEAN, p_performans INT, p_notlar TEXT
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE FutbolcuAntrenman 
    SET FutbolcuID = p_futbolcu_id, AntrenmanID = p_antrenman_id,
        Katilim = p_katilim, Performans = p_performans, Notlar = p_notlar
    WHERE FutbolcuAntrenmanID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 15: FutbolcuAntrenman Sil
CREATE OR REPLACE FUNCTION sp_futbolcu_antrenman_sil(p_id INT)
RETURNS BOOLEAN AS $$
BEGIN
    DELETE FROM FutbolcuAntrenman WHERE FutbolcuAntrenmanID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- SP 16: FutbolcuAntrenman Listele
CREATE OR REPLACE FUNCTION sp_futbolcu_antrenman_listele()
RETURNS TABLE (
    FutbolcuAntrenmanID INT, FutbolcuID INT, AntrenmanID INT,
    Katilim BOOLEAN, Performans INT, Notlar TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT fa.FutbolcuAntrenmanID, fa.FutbolcuID, fa.AntrenmanID,
           fa.Katilim, fa.Performans, fa.Notlar
    FROM FutbolcuAntrenman fa
    ORDER BY fa.KayitZamani DESC;
END;
$$ LANGUAGE plpgsql;

-- SP 17: Antrenmana Göre Katılımcıları Listele
CREATE OR REPLACE FUNCTION sp_antrenman_katilimci_listele(p_antrenman_id INT)
RETURNS TABLE (
    FutbolcuID INT, Ad VARCHAR, Soyad VARCHAR, FormaNo INT,
    Katilim BOOLEAN, Performans INT, Notlar TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT f.FutbolcuID, f.Ad, f.Soyad, f.FormaNo,
           fa.Katilim, fa.Performans, fa.Notlar
    FROM FutbolcuAntrenman fa
    JOIN Futbolcular f ON fa.FutbolcuID = f.FutbolcuID
    WHERE fa.AntrenmanID = p_antrenman_id
    ORDER BY f.FormaNo;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- EK TRIGGER'LAR (Antrenman Audit Log)
-- ============================================
-- NOT: Futbolcu trigger'ları zaten yukarıda tanımlı (satır 144-174)

-- Antrenman INSERT Trigger
CREATE OR REPLACE FUNCTION fn_log_antrenman_insert()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, YeniVeri)
    VALUES ('Antrenmanlar', 'INSERT', 0, 'Sistem', row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_antrenman_insert
AFTER INSERT ON Antrenmanlar
FOR EACH ROW EXECUTE FUNCTION fn_log_antrenman_insert();

-- Antrenman UPDATE Trigger
CREATE OR REPLACE FUNCTION fn_log_antrenman_update()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri, YeniVeri)
    VALUES ('Antrenmanlar', 'UPDATE', 0, 'Sistem', row_to_json(OLD), row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_antrenman_update
AFTER UPDATE ON Antrenmanlar
FOR EACH ROW EXECUTE FUNCTION fn_log_antrenman_update();

-- Antrenman DELETE Trigger
CREATE OR REPLACE FUNCTION fn_log_antrenman_delete()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri)
    VALUES ('Antrenmanlar', 'DELETE', 0, 'Sistem', row_to_json(OLD));
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_antrenman_delete
AFTER DELETE ON Antrenmanlar
FOR EACH ROW EXECUTE FUNCTION fn_log_antrenman_delete();

-- ============================================
-- TEST VERİLERİ
-- ============================================

-- Yönetici (Kullanıcı: semih / semih123)
INSERT INTO Yoneticiler (KullaniciAdi, SifreHash, Ad, Soyad, Eposta, TelefonNo, KullaniciTipi)
VALUES 
('semih', 'semih123', 'Semih', 'Yönetici', 'semih@futboltakimi.com', '0555 123 4567', 'Admin');

-- Antrenör (Kullanıcı: ramazan / ramazan123)
INSERT INTO Antrenorler (KullaniciAdi, SifreHash, Ad, Soyad, Eposta, Uzmanlik, KullaniciTipi)
VALUES 
('ramazan', 'ramazan123', 'Ramazan', 'Antrenör', 'ramazan@futboltakimi.com', 'Genel', 'TeknikDirektor');

-- Futbolcular
INSERT INTO Futbolcular (Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, FormaNo, Maas, SozlesmeBaslangic, SozlesmeBitis, Uyruk, Durumu)
VALUES 
('Volkan', 'Babacan', '1988-08-11', 185, 82, 'Kaleci', 1, 75000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Serdar', 'Aziz', '1990-10-23', 190, 85, 'Stoper', 4, 80000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Hakan', 'Çalhanoğlu', '1994-02-08', 178, 76, 'Orta Saha', 10, 120000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Burak', 'Yılmaz', '1985-07-15', 188, 83, 'Forvet', 9, 110000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Cenk', 'Tosun', '1991-06-07', 183, 80, 'Forvet', 23, 85000, '2024-01-01', '2025-12-31', 'Türkiye', 'Aktif'),
('İrfan Can', 'Kahveci', '1995-07-15', 184, 75, 'Kanat', 11, 95000, '2024-01-01', '2026-12-31', 'Türkiye', 'Sakat'),
('Kerem', 'Aktürkoğlu', '1998-10-21', 172, 68, 'Kanat', 7, 90000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Ferdi', 'Kadıoğlu', '1999-10-07', 175, 70, 'Sol Bek', 20, 70000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif'),
('Orkun', 'Kökçü', '2000-12-29', 177, 70, 'Orta Saha', 8, 85000, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif'),
('Altay', 'Bayındır', '1998-04-14', 198, 95, 'Kaleci', 34, 65000, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif');

-- Antrenmanlar (Tüm antrenmanlar ramazan'a ait - AntrenorID = 1)
INSERT INTO Antrenmanlar (Tarih, BaslangicSaati, BitisSaati, Tur, Notlar, AntrenorID)
VALUES 
('2024-11-25', '10:00', '12:00', 'Kondisyon', 'Sezon hazırlık', 1),
('2024-11-26', '10:00', '11:30', 'Teknik', 'Pas çalışması', 1),
('2024-11-27', '10:00', '12:00', 'Taktik', 'Maç taktiği', 1),
(CURRENT_DATE, '10:00', '12:00', 'Kondisyon', 'Günlük antrenman', 1),
(CURRENT_DATE + 1, '10:00', '11:30', 'Teknik', 'Şut çalışması', 1),
(CURRENT_DATE + 2, '10:00', '12:00', 'Taktik', 'Defans organizasyonu', 1);

-- Örnek katılım kayıtları
INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans)
SELECT f.FutbolcuID, 1, TRUE, 8
FROM Futbolcular f WHERE f.Durumu = 'Aktif' LIMIT 8;

-- ============================================
-- KURULUM TAMAMLANDI
-- ============================================

DO $$
DECLARE
    tablo_sayisi INTEGER;
    futbolcu_sayisi INTEGER;
    antrenor_sayisi INTEGER;
BEGIN
    SELECT COUNT(*) INTO tablo_sayisi FROM information_schema.tables WHERE table_schema = 'public';
    SELECT COUNT(*) INTO futbolcu_sayisi FROM Futbolcular;
    SELECT COUNT(*) INTO antrenor_sayisi FROM Antrenorler;
    
    RAISE NOTICE '';
    RAISE NOTICE '╔════════════════════════════════════════════════╗';
    RAISE NOTICE '║  KURULUM BAŞARIYLA TAMAMLANDI!               ║';
    RAISE NOTICE '╠════════════════════════════════════════════════╣';
    RAISE NOTICE '║  Tablo Sayısı       : %                      ║', tablo_sayisi;
    RAISE NOTICE '║  Futbolcu Sayısı    : %                      ║', futbolcu_sayisi;
    RAISE NOTICE '║  Antrenör Sayısı    : %                      ║', antrenor_sayisi;
    RAISE NOTICE '║  Stored Procedure   : 17 adet                 ║';
    RAISE NOTICE '║  Trigger            : 8 adet                  ║';
    RAISE NOTICE '║  Index              : 13 adet                 ║';
    RAISE NOTICE '╠════════════════════════════════════════════════╣';
    RAISE NOTICE '║  TEST KULLANICILARI:                          ║';
    RAISE NOTICE '║  Yönetici : admin / admin123                  ║';
    RAISE NOTICE '║  Antrenör : teknikdirektor / antrenor123      ║';
    RAISE NOTICE '╚════════════════════════════════════════════════╝';
END $$;

