-- ============================================
-- Futbol Takımı Yönetim Sistemi - PostgreSQL
-- Başlangıç Verileri ve Test Verileri
-- ============================================

\c futbol_takimi_db;

-- ============================================
-- NOT: Şifreler BCrypt ile hashlenmiş olmalı!
-- Test için geçici düz şifreler kullanıyoruz
-- Uygulama tarafında BCrypt ile hashlenecek
-- ============================================

-- Geçici olarak hashlenecek şifreler için fonksiyon
-- Gerçek uygulamada C# tarafında BCrypt.Net kullanılacak
-- Bu sadece test amaçlıdır

-- ============================================
-- 1. YÖNETİCİLER - Kullanıcılar
-- ============================================

-- Yönetici: semih / semih123
INSERT INTO Yoneticiler (KullaniciAdi, SifreHash, Ad, Soyad, Eposta, TelefonNo, KullaniciTipi, Aktif)
VALUES 
('semih', 'semih123', 'Semih', 'Yönetici', 'semih@futboltakimi.com', '0555 123 4567', 'Admin', TRUE);

COMMENT ON TABLE Yoneticiler IS 'Kullanıcı: semih / semih123';

-- ============================================
-- 2. ANTRENÖRLER - Kullanıcılar
-- ============================================

-- Antrenör: ramazan / ramazan123
INSERT INTO Antrenorler (KullaniciAdi, SifreHash, Ad, Soyad, Eposta, TelefonNo, Uzmanlik, KullaniciTipi, Aktif, IseBaslamaTarihi)
VALUES 
('ramazan', 'ramazan123', 'Ramazan', 'Antrenör', 'ramazan@futboltakimi.com', '0555 111 2222', 'Genel', 'TeknikDirektor', TRUE, '2024-01-01');

COMMENT ON TABLE Antrenorler IS 'Kullanıcı: ramazan / ramazan123';

-- ============================================
-- 3. FUTBOLCULAR - Test Verileri
-- ============================================

INSERT INTO Futbolcular (Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, FormaNo, Maas, SozlesmeBaslangic, SozlesmeBitis, Uyruk, Durumu, Notlar)
VALUES 
-- Kaleciler
('Volkan', 'Babacan', '1988-08-11', 185, 82, 'Kaleci', 1, 75000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Tecrübeli kaleci'),
('Altay', 'Bayındır', '1998-04-14', 198, 95, 'Kaleci', 34, 65000.00, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif', 'Genç ve yetenekli'),

-- Defans
('Serdar', 'Aziz', '1990-10-23', 190, 85, 'Stoper', 4, 80000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Defans organizatörü'),
('Marcão', 'Silva', '1996-06-05', 186, 83, 'Stoper', 3, 95000.00, '2024-01-01', '2026-12-31', 'Brezilya', 'Aktif', 'Güçlü stoper'),
('Ferdi', 'Kadıoğlu', '1999-10-07', 175, 70, 'Sol Bek', 20, 70000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Hızlı ve teknikli'),
('Samet', 'Akaydin', '1994-02-23', 188, 80, 'Sağ Bek', 2, 60000.00, '2024-01-01', '2025-12-31', 'Türkiye', 'Aktif', 'Savunmacı bek'),

-- Orta Saha
('Hakan', 'Çalhanoğlu', '1994-02-08', 178, 76, 'Ofansif Orta Saha', 10, 120000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Takım kaptanı'),
('Orkun', 'Kökçü', '2000-12-29', 177, 70, 'Orta Saha', 8, 85000.00, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif', 'Genç yetenek'),
('Salih', 'Özcan', '1998-01-11', 184, 78, 'Defansif Orta Saha', 6, 75000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Savunma yönü güçlü'),
('İrfan Can', 'Kahveci', '1995-07-15', 184, 75, 'Kanat', 11, 95000.00, '2024-01-01', '2026-12-31', 'Türkiye', 'Sakat', 'Sakatlık: Hamstring'),
('Kerem', 'Aktürkoğlu', '1998-10-21', 172, 68, 'Kanat', 7, 90000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Hücum oyuncusu'),

-- Forvetler
('Burak', 'Yılmaz', '1985-07-15', 188, 83, 'Santrafor', 9, 110000.00, '2023-07-01', '2024-12-31', 'Türkiye', 'Aktif', 'Tecrübeli golcü'),
('Cenk', 'Tosun', '1991-06-07', 183, 80, 'Forvet', 23, 85000.00, '2024-01-01', '2025-12-31', 'Türkiye', 'Aktif', 'Hava hakimiyeti iyi'),
('Bafetimbi', 'Gomis', '1985-08-06', 184, 82, 'Santrafor', 18, 100000.00, '2023-07-01', '2024-06-30', 'Fransa', 'Pasif', 'Sözleşme bitti'),

-- Yedekler
('Yunus', 'Akgün', '2000-07-07', 176, 70, 'Forvet', 19, 50000.00, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif', 'Genç forvet'),
('Berkan', 'Kutlu', '1998-05-07', 180, 73, 'Orta Saha', 5, 55000.00, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif', 'Orta saha rotasyon'),
('Emre', 'Kılınç', '1989-06-11', 175, 68, 'Kanat', 15, 60000.00, '2023-07-01', '2024-12-31', 'Türkiye', 'Cezalı', '2 maç ceza'),
('Alpaslan', 'Öztürk', '1997-12-05', 189, 84, 'Defans', 14, 55000.00, '2024-01-01', '2025-12-31', 'Türkiye', 'Aktif', 'Yedek stoper');

-- ============================================
-- 4. ANTRENMANLAR - Örnek Veriler
-- ============================================

INSERT INTO Antrenmanlar (Tarih, BaslangicSaati, BitisSaati, Tur, Notlar, AntrenorID, Lokasyon)
VALUES 
-- Geçmiş antrenmanlar (AntrenorID = 1, çünkü sadece ramazan var)
('2024-11-25', '10:00', '12:00', 'Kondisyon', 'Sezon hazırlık antreman - Koşu ve dayanıklılık çalışması', 1, 'Ana Tesis'),
('2024-11-26', '10:00', '11:30', 'Teknik', 'Pas çalışması ve top kontrolü', 1, 'Ana Tesis'),
('2024-11-27', '10:00', '12:00', 'Taktik', 'Maç taktiği ve set piece çalışmaları', 1, 'Ana Tesis'),
('2024-11-28', '10:00', '11:00', 'Toparlanma', 'Hafif antrenman - stretching ve masaj', 1, 'Ana Tesis'),
('2024-11-29', '15:00', '17:00', 'Hazırlık Maçı', 'A takım - Rezerv takım hazırlık maçı', 1, 'Stadyum'),

('2024-11-30', '10:00', '12:00', 'Kondisyon', 'Sprint ve ivmelenme çalışmaları', 1, 'Ana Tesis'),
('2024-12-01', '10:00', '11:30', 'Teknik', 'Şut çalışması ve bitiricilik', 1, 'Ana Tesis'),
('2024-12-02', '10:00', '12:00', 'Taktik', 'Defans organizasyonu', 1, 'Ana Tesis'),

-- Bugün ve yakın gelecek
(CURRENT_DATE, '10:00', '12:00', 'Kondisyon', 'Bugünkü antrenman - Genel kondisyon', 1, 'Ana Tesis'),
(CURRENT_DATE + 1, '10:00', '11:30', 'Teknik', 'Top çalışması', 1, 'Ana Tesis'),
(CURRENT_DATE + 2, '10:00', '12:00', 'Taktik', 'Maç hazırlık çalışması', 1, 'Ana Tesis'),
(CURRENT_DATE + 3, '15:00', '17:00', 'Hazırlık Maçı', 'Hazırlık maçı - B takımı', 1, 'Stadyum'),
(CURRENT_DATE + 4, '10:00', '11:00', 'Toparlanma', 'Dinlenme antreman', 1, 'Ana Tesis'),
(CURRENT_DATE + 5, '10:00', '12:00', 'Kondisyon', 'Haftalık kondisyon antreman', 1, 'Ana Tesis');

-- ============================================
-- 5. FUTBOLCU-ANTRENMAN İLİŞKİLERİ
-- ============================================

-- 25 Kasım 2024 - Kondisyon antreman
INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
SELECT f.FutbolcuID, 1, 
    CASE WHEN f.Durumu = 'Aktif' THEN TRUE ELSE FALSE END,
    CASE WHEN f.Durumu = 'Aktif' THEN (6 + (RANDOM() * 4)::INTEGER) ELSE NULL END,
    CASE WHEN f.Durumu != 'Aktif' THEN 'Antrenmana katılmadı: ' || f.Durumu ELSE 'İyi performans' END
FROM Futbolcular f
WHERE f.FutbolcuID <= 10;

-- 26 Kasım 2024 - Teknik antrenman
INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
SELECT f.FutbolcuID, 2, 
    CASE WHEN f.Durumu = 'Aktif' THEN TRUE ELSE FALSE END,
    CASE WHEN f.Durumu = 'Aktif' THEN (7 + (RANDOM() * 3)::INTEGER) ELSE NULL END,
    'Pas çalışması'
FROM Futbolcular f
WHERE f.FutbolcuID <= 12;

-- 27 Kasım 2024 - Taktik antrenman
INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
SELECT f.FutbolcuID, 3, TRUE, (6 + (RANDOM() * 4)::INTEGER), 'Taktik çalışma'
FROM Futbolcular f
WHERE f.Durumu = 'Aktif';

-- Bugünkü antrenman (katılım henüz girilmemiş)
INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
SELECT f.FutbolcuID, 9, FALSE, NULL, 'Katılım henüz girilmedi'
FROM Futbolcular f
WHERE f.Durumu = 'Aktif';

-- ============================================
-- 6. BİLDİRİMLER - Örnek Uyarılar
-- ============================================

-- Sözleşmesi bitmek üzere olan futbolcular için bildirim
INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
SELECT 'SozlesmeBitiyor', FutbolcuID, 
    'DİKKAT: ' || Ad || ' ' || Soyad || ' isimli futbolcunun sözleşmesi ' || 
    TO_CHAR(SozlesmeBitis, 'DD.MM.YYYY') || ' tarihinde bitiyor!',
    FALSE
FROM Futbolcular
WHERE SozlesmeBitis BETWEEN CURRENT_DATE AND CURRENT_DATE + INTERVAL '6 months';

-- Sakatlar için bildirim
INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
SELECT 'SakatlikUyarisi', FutbolcuID,
    Ad || ' ' || Soyad || ' sakat durumda. Notlar: ' || COALESCE(Notlar, 'Belirtilmemiş'),
    FALSE
FROM Futbolcular
WHERE Durumu = 'Sakat';

-- Cezalılar için bildirim
INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
SELECT 'CezaSuresi', FutbolcuID,
    Ad || ' ' || Soyad || ' cezalı durumda. Detay: ' || COALESCE(Notlar, 'Belirtilmemiş'),
    FALSE
FROM Futbolcular
WHERE Durumu = 'Cezalı';

-- Genel sistem bildirimi
INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
VALUES 
('Genel', NULL, 'Sistem başarıyla kuruldu. Futbol Takımı Yönetim Sistemi''ne hoş geldiniz!', FALSE),
('Genel', NULL, 'Yeni sezon hazırlıkları başladı. Tüm futbolculara kondisyon testleri yapılacaktır.', FALSE);

-- ============================================
-- 7. İSTATİSTİKLER VE ÖZET
-- ============================================

DO $$
DECLARE
    yonetici_count INTEGER;
    antrenor_count INTEGER;
    futbolcu_count INTEGER;
    aktif_futbolcu_count INTEGER;
    antrenman_count INTEGER;
    katilim_count INTEGER;
    bildirim_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO yonetici_count FROM Yoneticiler;
    SELECT COUNT(*) INTO antrenor_count FROM Antrenorler;
    SELECT COUNT(*) INTO futbolcu_count FROM Futbolcular;
    SELECT COUNT(*) INTO aktif_futbolcu_count FROM Futbolcular WHERE Durumu = 'Aktif';
    SELECT COUNT(*) INTO antrenman_count FROM Antrenmanlar;
    SELECT COUNT(*) INTO katilim_count FROM FutbolcuAntrenman;
    SELECT COUNT(*) INTO bildirim_count FROM Bildirimler;
    
    RAISE NOTICE '╔════════════════════════════════════════════════════════╗';
    RAISE NOTICE '║   BAŞLANGIÇ VERİLERİ BAŞARIYLA YÜKLENDİ!            ║';
    RAISE NOTICE '╠════════════════════════════════════════════════════════╣';
    RAISE NOTICE '║   Yönetici Sayısı        : % ', yonetici_count;
    RAISE NOTICE '║   Antrenör Sayısı        : % ', antrenor_count;
    RAISE NOTICE '║   Futbolcu Sayısı        : % ', futbolcu_count;
    RAISE NOTICE '║   Aktif Futbolcu Sayısı  : % ', aktif_futbolcu_count;
    RAISE NOTICE '║   Antrenman Sayısı       : % ', antrenman_count;
    RAISE NOTICE '║   Katılım Kaydı Sayısı   : % ', katilim_count;
    RAISE NOTICE '║   Bildirim Sayısı        : % ', bildirim_count;
    RAISE NOTICE '╠════════════════════════════════════════════════════════╣';
    RAISE NOTICE '║   KULLANICI BİLGİLERİ:                               ║';
    RAISE NOTICE '║   Yönetici   : semih / semih123                       ║';
    RAISE NOTICE '║   Antrenör   : ramazan / ramazan123                   ║';
    RAISE NOTICE '╚════════════════════════════════════════════════════════╝';
END $$;

