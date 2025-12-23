-- ============================================
-- Stored Procedure: Futbolcu Güncelle
-- ============================================
-- Mevcut futbolcu kaydını günceller, başarılı ise TRUE döndürür

CREATE OR REPLACE FUNCTION sp_futbolcu_guncelle(
    p_id INT, p_ad VARCHAR, p_soyad VARCHAR, p_dogum DATE, p_boy INT, p_kilo INT,
    p_pozisyon VARCHAR, p_forma_no INT, p_maas DECIMAL, 
    p_sozlesme_bas DATE, p_sozlesme_bit DATE, p_uyruk VARCHAR, p_durumu VARCHAR, p_notlar TEXT
)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE Futbolcular 
    SET Ad = p_ad, Soyad = p_soyad, DogumTarihi = p_dogum, Boy = p_boy, 
        Kilo = p_kilo, Pozisyon = p_pozisyon, FormaNo = p_forma_no, Maas = p_maas,
        SozlesmeBaslangic = p_sozlesme_bas, SozlesmeBitis = p_sozlesme_bit,
        Uyruk = p_uyruk, Durumu = p_durumu, Notlar = p_notlar
    WHERE FutbolcuID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT sp_futbolcu_guncelle(1, 'Volkan', 'Babacan', '1988-08-11', 185, 82, 'Kaleci', 1, 80000, '2023-07-01', '2025-06-30', 'Türkiye', 'Aktif');

