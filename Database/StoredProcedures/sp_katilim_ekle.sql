-- ============================================
-- Stored Procedure: Antrenman Katılım Ekle
-- ============================================
-- Futbolcunun antrenmana katılımını ve performansını kaydeder

CREATE OR REPLACE FUNCTION sp_katilim_ekle(
    p_futbolcu_id INT, p_antrenman_id INT, 
    p_katilim BOOLEAN, p_performans INT, p_notlar TEXT
)
RETURNS INTEGER AS $$
DECLARE
    v_id INTEGER;
BEGIN
    INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans, Notlar)
    VALUES (p_futbolcu_id, p_antrenman_id, p_katilim, p_performans, p_notlar)
    RETURNING FutbolcuAntrenmanID INTO v_id;
    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT sp_katilim_ekle(1, 1, TRUE, 8, 'Çok başarılı bir antrenman');

