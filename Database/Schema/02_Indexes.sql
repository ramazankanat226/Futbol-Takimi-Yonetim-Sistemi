
CREATE INDEX idx_yonetici_kullanici_adi ON Yoneticiler(KullaniciAdi);
CREATE INDEX idx_yonetici_aktif ON Yoneticiler(Aktif) WHERE Aktif = TRUE;
CREATE INDEX idx_yonetici_eposta ON Yoneticiler(Eposta);

COMMENT ON INDEX idx_yonetici_kullanici_adi IS 'Kullanıcı adı ile hızlı giriş sorguları için';
COMMENT ON INDEX idx_yonetici_aktif IS 'Sadece aktif yöneticileri filtrelemek için partial index';

-- ============================================
-- Antrenorler Tablosu İndeksleri
-- ============================================

CREATE INDEX idx_antrenor_kullanici_adi ON Antrenorler(KullaniciAdi);
CREATE INDEX idx_antrenor_uzmanlik ON Antrenorler(Uzmanlik);
CREATE INDEX idx_antrenor_aktif ON Antrenorler(Aktif) WHERE Aktif = TRUE;
CREATE INDEX idx_antrenor_kullanici_tipi ON Antrenorler(KullaniciTipi);

COMMENT ON INDEX idx_antrenor_uzmanlik IS 'Uzmanlık alanına göre antrenör filtreleme için';

-- ============================================
-- Futbolcular Tablosu İndeksleri
-- ============================================

-- En çok kullanılan sorgular için
CREATE INDEX idx_futbolcu_pozisyon ON Futbolcular(Pozisyon);
CREATE INDEX idx_futbolcu_durumu ON Futbolcular(Durumu);
CREATE INDEX idx_futbolcu_forma_no ON Futbolcular(FormaNo);
CREATE INDEX idx_futbolcu_uyruk ON Futbolcular(Uyruk);

-- Sözleşme sorguları için
CREATE INDEX idx_futbolcu_sozlesme_bitis ON Futbolcular(SozlesmeBitis);
CREATE INDEX idx_futbolcu_sozlesme_baslangic ON Futbolcular(SozlesmeBaslangic);

-- Tam metin arama için (Ad Soyad)
CREATE INDEX idx_futbolcu_ad_soyad ON Futbolcular(Ad, Soyad);
CREATE INDEX idx_futbolcu_soyad_ad ON Futbolcular(Soyad, Ad);

-- Doğum tarihi sorguları için
CREATE INDEX idx_futbolcu_dogum_tarihi ON Futbolcular(DogumTarihi);

-- Sadece aktif futbolcular için partial index
CREATE INDEX idx_futbolcu_aktif ON Futbolcular(FutbolcuID) WHERE Durumu = 'Aktif';

-- Composite index: Pozisyon + Durum (çok kullanılan kombinasyon)
CREATE INDEX idx_futbolcu_pozisyon_durumu ON Futbolcular(Pozisyon, Durumu);

COMMENT ON INDEX idx_futbolcu_pozisyon IS 'Pozisyona göre filtreleme için';
COMMENT ON INDEX idx_futbolcu_durumu IS 'Durum bazlı sorgular için (Aktif, Sakat, vb.)';
COMMENT ON INDEX idx_futbolcu_sozlesme_bitis IS 'Sözleşmesi bitmek üzere olan futbolcuları bulmak için';
COMMENT ON INDEX idx_futbolcu_aktif IS 'Sadece aktif futbolcular için optimizasyon';

-- ============================================
-- Antrenmanlar Tablosu İndeksleri
-- ============================================

-- Tarih bazlı sorgular için
CREATE INDEX idx_antrenman_tarih ON Antrenmanlar(Tarih DESC);
CREATE INDEX idx_antrenman_tarih_tur ON Antrenmanlar(Tarih, Tur);

-- Antrenman türü için
CREATE INDEX idx_antrenman_tur ON Antrenmanlar(Tur);

-- Antrenör bazlı sorgular için
CREATE INDEX idx_antrenman_antrenor ON Antrenmanlar(AntrenorID);

-- Yakın tarihli antrenmanlar için partial index
CREATE INDEX idx_antrenman_gelecek ON Antrenmanlar(Tarih) 
    WHERE Tarih >= CURRENT_DATE;

-- Geçmiş antrenmanlar için partial index
CREATE INDEX idx_antrenman_gecmis ON Antrenmanlar(Tarih) 
    WHERE Tarih < CURRENT_DATE;

COMMENT ON INDEX idx_antrenman_tarih IS 'Tarih sıralı listeler için (DESC sıralama)';
COMMENT ON INDEX idx_antrenman_gelecek IS 'Gelecek antrenmanları hızlı listelemek için';
COMMENT ON INDEX idx_antrenman_gecmis IS 'Geçmiş antrenman raporları için';

-- ============================================
-- FutbolcuAntrenman Tablosu İndeksleri
-- ============================================

-- Foreign key ilişkileri için
CREATE INDEX idx_futbolcu_antrenman_futbolcu ON FutbolcuAntrenman(FutbolcuID);
CREATE INDEX idx_futbolcu_antrenman_antrenman ON FutbolcuAntrenman(AntrenmanID);

-- Katılım raporları için
CREATE INDEX idx_futbolcu_antrenman_katilim ON FutbolcuAntrenman(Katilim);
CREATE INDEX idx_futbolcu_antrenman_performans ON FutbolcuAntrenman(Performans) 
    WHERE Performans IS NOT NULL;

-- Composite index: Futbolcu + Katılım durumu
CREATE INDEX idx_futbolcu_antrenman_futbolcu_katilim ON FutbolcuAntrenman(FutbolcuID, Katilim);

-- Composite index: Antrenman + Katılım durumu
CREATE INDEX idx_futbolcu_antrenman_antrenman_katilim ON FutbolcuAntrenman(AntrenmanID, Katilim);

COMMENT ON INDEX idx_futbolcu_antrenman_futbolcu IS 'Futbolcuya göre antrenman geçmişi sorgular için';
COMMENT ON INDEX idx_futbolcu_antrenman_antrenman IS 'Antrenmana göre katılımcı listesi için';
COMMENT ON INDEX idx_futbolcu_antrenman_performans IS 'Performans değerlendirme raporları için';

-- ============================================
-- LogTablosu İndeksleri
-- ============================================

-- Zaman bazlı sorgular için
CREATE INDEX idx_log_islem_zamani ON LogTablosu(IslemZamani DESC);

-- Tablo ve işlem bazlı sorgular için
CREATE INDEX idx_log_tablo_adi ON LogTablosu(TabloAdi);
CREATE INDEX idx_log_islem ON LogTablosu(Islem);
CREATE INDEX idx_log_tablo_islem ON LogTablosu(TabloAdi, Islem);

-- Kullanıcı bazlı sorgular için
CREATE INDEX idx_log_kullanici ON LogTablosu(KullaniciID, KullaniciTipi);

-- Son 30 günlük loglar için partial index
CREATE INDEX idx_log_son_30_gun ON LogTablosu(IslemZamani DESC) 
    WHERE IslemZamani >= CURRENT_TIMESTAMP - INTERVAL '30 days';

-- JSONB sütunları için GIN index (hızlı JSON arama)
CREATE INDEX idx_log_eski_veri_gin ON LogTablosu USING GIN (EskiVeri);
CREATE INDEX idx_log_yeni_veri_gin ON LogTablosu USING GIN (YeniVeri);

COMMENT ON INDEX idx_log_islem_zamani IS 'Zaman sıralı log sorguları için';
COMMENT ON INDEX idx_log_son_30_gun IS 'Son 30 günlük logları hızlı getirmek için';
COMMENT ON INDEX idx_log_eski_veri_gin IS 'JSON verilerde hızlı arama için GIN index';

-- ============================================
-- Bildirimler Tablosu İndeksleri
-- ============================================

-- Bildirim sorguları için
CREATE INDEX idx_bildirim_futbolcu ON Bildirimler(FutbolcuID);
CREATE INDEX idx_bildirim_okundu ON Bildirimler(Okundu) WHERE Okundu = FALSE;
CREATE INDEX idx_bildirim_baslik_tipi ON Bildirimler(BaslikTipi);
CREATE INDEX idx_bildirim_olusturma_tarihi ON Bildirimler(OlusturmaTarihi DESC);

-- Composite index: Okunmamış bildirimler
CREATE INDEX idx_bildirim_okunmamis_tarih ON Bildirimler(OlusturmaTarihi DESC) 
    WHERE Okundu = FALSE;

COMMENT ON INDEX idx_bildirim_okundu IS 'Okunmamış bildirimleri hızlı listelemek için';
COMMENT ON INDEX idx_bildirim_okunmamis_tarih IS 'Okunmamış bildirimleri tarih sıralı getirmek için';

-- ============================================
-- Full Text Search İndeksleri (Gelişmiş Arama)
-- ============================================

-- Futbolcu tam metin arama için
CREATE INDEX idx_futbolcu_fulltext ON Futbolcular 
    USING GIN (to_tsvector('turkish', Ad || ' ' || Soyad || ' ' || COALESCE(Notlar, '')));

-- Antrenman notları arama için
CREATE INDEX idx_antrenman_fulltext ON Antrenmanlar 
    USING GIN (to_tsvector('turkish', COALESCE(Notlar, '')));

COMMENT ON INDEX idx_futbolcu_fulltext IS 'Futbolcu ad, soyad ve notlarda Türkçe tam metin arama için';
COMMENT ON INDEX idx_antrenman_fulltext IS 'Antrenman notlarında Türkçe tam metin arama için';

-- ============================================
-- İstatistikleri güncelle
-- ============================================

ANALYZE Yoneticiler;
ANALYZE Antrenorler;
ANALYZE Futbolcular;
ANALYZE Antrenmanlar;
ANALYZE FutbolcuAntrenman;
ANALYZE LogTablosu;
ANALYZE Bildirimler;

-- Başarı mesajı
DO $$
DECLARE
    index_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO index_count 
    FROM pg_indexes 
    WHERE schemaname = 'public';
    
    RAISE NOTICE 'İndeksler başarıyla oluşturuldu!';
    RAISE NOTICE 'Toplam indeks sayısı: %', index_count;
    RAISE NOTICE 'Performans optimizasyonu tamamlandı.';
END $$;

