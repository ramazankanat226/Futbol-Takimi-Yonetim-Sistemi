-- ============================================
-- Stored Procedure: Antrenman Ekle
-- ============================================
-- Yeni antrenman kaydı oluşturur ve AntrenmanID döndürür

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

-- Örnek Kullanım:
-- SELECT sp_antrenman_ekle('2024-12-23', '10:00', '12:00', 'Kondisyon', 'Yoğun tempo antrenmanı', 1);

