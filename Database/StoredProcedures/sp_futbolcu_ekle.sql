-- ============================================
-- Stored Procedure: Futbolcu Ekle
-- ============================================
-- Yeni futbolcu kaydı oluşturur ve FutbolcuID döndürür

CREATE OR REPLACE FUNCTION sp_futbolcu_ekle(
    p_ad VARCHAR, p_soyad VARCHAR, p_dogum DATE, p_boy INT, p_kilo INT,
    p_pozisyon VARCHAR, p_forma_no INT, p_maas DECIMAL, 
    p_sozlesme_bas DATE, p_sozlesme_bit DATE, p_uyruk VARCHAR, p_durumu VARCHAR, p_notlar TEXT
)
RETURNS INTEGER AS $$
DECLARE
    v_futbolcu_id INTEGER;
BEGIN
    INSERT INTO Futbolcular (Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, FormaNo, 
                             Maas, SozlesmeBaslangic, SozlesmeBitis, Uyruk, Durumu, Notlar)
    VALUES (p_ad, p_soyad, p_dogum, p_boy, p_kilo, p_pozisyon, p_forma_no,
            p_maas, p_sozlesme_bas, p_sozlesme_bit, p_uyruk, p_durumu, p_notlar)
    RETURNING FutbolcuID INTO v_futbolcu_id;
    RETURN v_futbolcu_id;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT sp_futbolcu_ekle('Arda', 'Güler', '2005-02-25', 175, 70, 'Orta Saha', 15, 50000, '2024-01-01', '2026-12-31', 'Türkiye', 'Aktif');

