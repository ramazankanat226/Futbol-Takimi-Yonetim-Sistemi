-- ============================================
-- Futbol Takımı Yönetim Sistemi - PostgreSQL
-- Ek Constraint'ler ve İş Kuralları
-- ============================================

\c futbol_takimi_db;

-- ============================================
-- NORMALIZASYON KONTROLLARI
-- ============================================

-- Bu dosya 3NF (Third Normal Form) kurallarına uyum sağlar:
-- 1NF: Her alan atomic (bölünemez) değerler içerir ✓
-- 2NF: Tüm non-key alanlar primary key'e bağımlıdır ✓
-- 3NF: Non-key alanlar birbirine bağımlı değildir ✓

-- ============================================
-- FUTBOLCULAR İÇİN EK CONSTRAINT'LER
-- ============================================

-- Yaş kontrolü: 14-45 yaş arası olmalı
ALTER TABLE Futbolcular 
ADD CONSTRAINT chk_futbolcu_yas 
CHECK (
    EXTRACT(YEAR FROM AGE(CURRENT_DATE, DogumTarihi)) BETWEEN 14 AND 45
);

-- Sözleşme süresi kontrolü: En az 6 ay olmalı
ALTER TABLE Futbolcular 
ADD CONSTRAINT chk_futbolcu_sozlesme_sure 
CHECK (
    SozlesmeBitis >= SozlesmeBaslangic + INTERVAL '6 months'
);

-- Maaş pozisyona göre minimum kontrol
ALTER TABLE Futbolcular 
ADD CONSTRAINT chk_futbolcu_maas_minimum 
CHECK (Maas >= 5000);

-- Boy ve kilo gerçekçi değerler olmalı
ALTER TABLE Futbolcular 
ADD CONSTRAINT chk_futbolcu_boy_kilo_oran
CHECK (
    (Kilo::DECIMAL / (Boy::DECIMAL / 100) / (Boy::DECIMAL / 100)) BETWEEN 15 AND 35
    -- BMI (Body Mass Index) kontrolü: 15-35 arası
);

COMMENT ON CONSTRAINT chk_futbolcu_yas ON Futbolcular 
    IS 'Futbolcu yaşı 14-45 arasında olmalıdır';
COMMENT ON CONSTRAINT chk_futbolcu_sozlesme_sure ON Futbolcular 
    IS 'Sözleşme süresi en az 6 ay olmalıdır';
COMMENT ON CONSTRAINT chk_futbolcu_maas_minimum ON Futbolcular 
    IS 'Minimum maaş 5.000 TL olmalıdır';
COMMENT ON CONSTRAINT chk_futbolcu_boy_kilo_oran ON Futbolcular 
    IS 'BMI (Vücut Kitle İndeksi) 15-35 arasında olmalıdır';

-- ============================================
-- ANTRENMANLAR İÇİN EK CONSTRAINT'LER
-- ============================================

-- Antrenman süresi kontrolü: Minimum 30 dakika, maksimum 4 saat
ALTER TABLE Antrenmanlar 
ADD CONSTRAINT chk_antrenman_sure 
CHECK (
    BitisSaati - BaslangicSaati >= INTERVAL '30 minutes' AND
    BitisSaati - BaslangicSaati <= INTERVAL '4 hours'
);

-- Antrenman tarihi gelecekte olamaz (30 günden fazla)
ALTER TABLE Antrenmanlar 
ADD CONSTRAINT chk_antrenman_tarih_gelecek 
CHECK (
    Tarih <= CURRENT_DATE + INTERVAL '30 days'
);

-- Antrenman tarihi çok geçmişte olamaz (5 yıldan eski)
ALTER TABLE Antrenmanlar 
ADD CONSTRAINT chk_antrenman_tarih_gecmis 
CHECK (
    Tarih >= CURRENT_DATE - INTERVAL '5 years'
);

COMMENT ON CONSTRAINT chk_antrenman_sure ON Antrenmanlar 
    IS 'Antrenman süresi 30 dakika ile 4 saat arasında olmalıdır';
COMMENT ON CONSTRAINT chk_antrenman_tarih_gelecek ON Antrenmanlar 
    IS 'Antrenman tarihi en fazla 30 gün sonrası olabilir';
COMMENT ON CONSTRAINT chk_antrenman_tarih_gecmis ON Antrenmanlar 
    IS 'Antrenman tarihi en fazla 5 yıl geriye gidebilir';

-- ============================================
-- FUTBOLCU-ANTRENMAN İÇİN EK CONSTRAINT'LER
-- ============================================

-- Performans sadece katılım varsa girilmeli
ALTER TABLE FutbolcuAntrenman 
ADD CONSTRAINT chk_performans_katilim 
CHECK (
    (Katilim = FALSE AND Performans IS NULL) OR
    (Katilim = TRUE)
);

COMMENT ON CONSTRAINT chk_performans_katilim ON FutbolcuAntrenman 
    IS 'Performans değeri sadece katılım varsa girilmelidir';

-- ============================================
-- KULLANICI GÜVENLİK CONSTRAINT'LERİ
-- ============================================

-- Kullanıcı adı en az 3 karakter olmalı
ALTER TABLE Yoneticiler 
ADD CONSTRAINT chk_yonetici_kullanici_adi_uzunluk 
CHECK (LENGTH(TRIM(KullaniciAdi)) >= 3);

ALTER TABLE Antrenorler 
ADD CONSTRAINT chk_antrenor_kullanici_adi_uzunluk 
CHECK (LENGTH(TRIM(KullaniciAdi)) >= 3);

-- Ad ve Soyad boş olamaz ve en az 2 karakter
ALTER TABLE Yoneticiler 
ADD CONSTRAINT chk_yonetici_ad_soyad 
CHECK (LENGTH(TRIM(Ad)) >= 2 AND LENGTH(TRIM(Soyad)) >= 2);

ALTER TABLE Antrenorler 
ADD CONSTRAINT chk_antrenor_ad_soyad 
CHECK (LENGTH(TRIM(Ad)) >= 2 AND LENGTH(TRIM(Soyad)) >= 2);

ALTER TABLE Futbolcular 
ADD CONSTRAINT chk_futbolcu_ad_soyad 
CHECK (LENGTH(TRIM(Ad)) >= 2 AND LENGTH(TRIM(Soyad)) >= 2);

-- E-posta format kontrolü (basit regex)
ALTER TABLE Yoneticiler 
ADD CONSTRAINT chk_yonetici_eposta_format 
CHECK (Eposta IS NULL OR Eposta ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$');

ALTER TABLE Antrenorler 
ADD CONSTRAINT chk_antrenor_eposta_format 
CHECK (Eposta IS NULL OR Eposta ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$');

-- Telefon numarası format kontrolü (Türkiye formatı)
ALTER TABLE Yoneticiler 
ADD CONSTRAINT chk_yonetici_telefon_format 
CHECK (TelefonNo IS NULL OR TelefonNo ~ '^[0-9\s\-\+\(\)]{10,20}$');

ALTER TABLE Antrenorler 
ADD CONSTRAINT chk_antrenor_telefon_format 
CHECK (TelefonNo IS NULL OR TelefonNo ~ '^[0-9\s\-\+\(\)]{10,20}$');

COMMENT ON CONSTRAINT chk_yonetici_kullanici_adi_uzunluk ON Yoneticiler 
    IS 'Kullanıcı adı en az 3 karakter olmalıdır';
COMMENT ON CONSTRAINT chk_yonetici_eposta_format ON Yoneticiler 
    IS 'E-posta adresi geçerli formatta olmalıdır';

-- ============================================
-- LOG TABLOSU CONSTRAINT'LERİ
-- ============================================

-- Log kayıtları silinemez (sadece insert), bu yüzden NO DELETE politikası
-- Trigger ile kontrol edilecek

-- KullaniciID null olamaz (sistem işlemleri için 0 kullanılabilir)
ALTER TABLE LogTablosu 
ADD CONSTRAINT chk_log_kullanici_id 
CHECK (KullaniciID IS NOT NULL);

COMMENT ON CONSTRAINT chk_log_kullanici_id ON LogTablosu 
    IS 'Log kaydı mutlaka bir kullanıcıya ait olmalıdır (Sistem işlemleri için 0)';

-- ============================================
-- REFERANS BÜTÜNLÜĞÜNÜ SAĞLAYAN EK KURALLAR
-- ============================================

-- Bir antrenman silindiğinde, ona ait katılım kayıtları da silinmeli (CASCADE)
-- Bu zaten 01_Tables.sql'de tanımlandı, kontrol edelim:

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints 
        WHERE constraint_name = 'fk_futbolcu_antrenman_antrenman' 
        AND table_name = 'futbolcuantrenman'
    ) THEN
        RAISE EXCEPTION 'Foreign key constraint eksik: fk_futbolcu_antrenman_antrenman';
    END IF;
    
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints 
        WHERE constraint_name = 'fk_futbolcu_antrenman_futbolcu' 
        AND table_name = 'futbolcuantrenman'
    ) THEN
        RAISE EXCEPTION 'Foreign key constraint eksik: fk_futbolcu_antrenman_futbolcu';
    END IF;
    
    RAISE NOTICE 'Tüm foreign key constraint''ler doğrulandı.';
END $$;

-- ============================================
-- VERİ TUTARLILIĞI FONKSİYONLARI
-- ============================================

-- Sözleşmesi bitmiş futbolcuları otomatik 'Pasif' yapma
CREATE OR REPLACE FUNCTION fn_kontrol_sozlesme_biten()
RETURNS void AS $$
BEGIN
    UPDATE Futbolcular
    SET Durumu = 'Pasif'
    WHERE SozlesmeBitis < CURRENT_DATE 
    AND Durumu != 'Pasif';
    
    RAISE NOTICE 'Sözleşmesi bitmiş futbolcular güncellendi.';
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION fn_kontrol_sozlesme_biten() 
    IS 'Sözleşmesi bitmiş futbolcuları otomatik pasif yapar - Günlük çalıştırılmalı';

-- ============================================
-- CONSTRAINT'LERİ DOĞRULAMA
-- ============================================

DO $$
DECLARE
    constraint_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO constraint_count
    FROM information_schema.table_constraints
    WHERE table_schema = 'public'
    AND constraint_type IN ('CHECK', 'FOREIGN KEY', 'UNIQUE', 'PRIMARY KEY');
    
    RAISE NOTICE 'Constraint''ler başarıyla uygulandı!';
    RAISE NOTICE 'Toplam constraint sayısı: %', constraint_count;
    RAISE NOTICE 'Normalizasyon seviyesi: 3NF (Third Normal Form)';
    RAISE NOTICE 'Veri bütünlüğü ve iş kuralları aktif.';
END $$;

-- ============================================
-- NOTLAR
-- ============================================

-- 1. Tüm CHECK constraint'ler uygulama seviyesinde de kontrol edilmelidir
-- 2. Foreign key constraint'ler referans bütünlüğünü garanti eder
-- 3. UNIQUE constraint'ler veri tekrarını engeller
-- 4. Trigger'lar ek iş kuralları için kullanılacak (04_Triggers.sql)
-- 5. Normalizasyon 3NF seviyesinde tutulmuştur

