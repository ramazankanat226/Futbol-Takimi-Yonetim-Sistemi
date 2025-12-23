-- ============================================
-- Stored Procedure: Sözleşmesi Biten Futbolcular
-- ============================================
-- 90 gün içinde sözleşmesi bitecek futbolcuları listeler

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

-- Örnek Kullanım:
-- SELECT * FROM sp_sozlesme_biten_futbolcular();

