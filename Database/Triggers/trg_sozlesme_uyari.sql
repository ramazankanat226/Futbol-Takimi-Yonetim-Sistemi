CREATE OR REPLACE FUNCTION fn_sozlesme_uyari()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.SozlesmeBitis <= CURRENT_DATE + INTERVAL '90 days' THEN
        INSERT INTO Bildirimler (BaslikTipi, FutbolcuID, Mesaj, Okundu)
        VALUES ('SozlesmeBitiyor', NEW.FutbolcuID, 
                NEW.Ad || ' ' || NEW.Soyad || ' isimli futbolcunun sözleşmesi ' || 
                TO_CHAR(NEW.SozlesmeBitis, 'DD.MM.YYYY') || ' tarihinde bitiyor!', FALSE);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_sozlesme_uyari
AFTER INSERT OR UPDATE ON Futbolcular
FOR EACH ROW EXECUTE FUNCTION fn_sozlesme_uyari();

