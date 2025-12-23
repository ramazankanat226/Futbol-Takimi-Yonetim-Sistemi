-- ============================================
-- Stored Procedure: Futbolcu Listele
-- ============================================
-- Tüm futbolcuları soyad ve ada göre sıralı listeler

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

-- Örnek Kullanım:
-- SELECT * FROM sp_futbolcu_listele();

