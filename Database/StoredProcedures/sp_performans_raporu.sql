-- ============================================
-- Stored Procedure: Futbolcu Performans Raporu
-- ============================================
-- Belirli bir futbolcunun tüm antrenman katılım ve performans geçmişini listeler

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

-- Örnek Kullanım:
-- SELECT * FROM sp_performans_raporu(1);

