-- ============================================
-- Trigger: Futbolcu Ekleme Logu
-- ============================================
-- Her futbolcu eklendiğinde LogTablosu'na kayıt atar

CREATE OR REPLACE FUNCTION fn_log_futbolcu_insert()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO LogTablosu (TabloAdi, Islem, KullaniciID, KullaniciTipi, YeniVeri)
    VALUES ('Futbolcular', 'INSERT', 0, 'Sistem', row_to_json(NEW));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_futbolcu_insert
AFTER INSERT ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_log_futbolcu_insert();

