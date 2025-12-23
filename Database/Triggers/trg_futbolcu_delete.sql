-- ============================================
-- Trigger: Futbolcu Silme Logu
-- ============================================
-- Her futbolcu silindiğinde eski veriyi LogTablosu'na kaydeder

CREATE OR REPLACE FUNCTION fn_log_futbolcu_delete()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri)
    VALUES ('Futbolcular', 'DELETE', 0, 'Sistem', row_to_json(OLD));
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_delete
AFTER DELETE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_delete();

