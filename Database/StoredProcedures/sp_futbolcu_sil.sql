-- ============================================
-- Stored Procedure: Futbolcu Sil
-- ============================================
-- Futbolcu kaydını siler (CASCADE ile ilişkili kayıtlar da silinir)

CREATE OR REPLACE FUNCTION sp_futbolcu_sil(p_id INT)
RETURNS BOOLEAN AS $$
BEGIN
    DELETE FROM Futbolcular WHERE FutbolcuID = p_id;
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT sp_futbolcu_sil(1);

