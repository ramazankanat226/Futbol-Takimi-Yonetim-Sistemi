-- ============================================
-- Trigger: Futbolcu Güncelleme Logu
-- ============================================
-- Her futbolcu güncellendiğinde eski ve yeni veriyi LogTablosu'na kaydeder

CREATE OR REPLACE FUNCTION fn_log_futbolcu_update()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, EskiVeri, YeniVeri)
    VALUES ('Futbolcular', 'UPDATE', 0, 'Sistem', row_to_json(OLD), row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_update
AFTER UPDATE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_update();

