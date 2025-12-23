-- ============================================
-- Stored Procedure: Yönetici Giriş Kontrolü
-- ============================================
-- Kullanıcı adı ve şifre kontrolü yapar, başarılı ise yönetici bilgilerini döndürür

CREATE OR REPLACE FUNCTION sp_yonetici_giris(p_kullanici VARCHAR, p_sifre VARCHAR)
RETURNS TABLE (
    YoneticiID INT, KullaniciAdi VARCHAR, Ad VARCHAR, 
    Soyad VARCHAR, KullaniciTipi VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT y.YoneticiID, y.KullaniciAdi, y.Ad, y.Soyad, y.KullaniciTipi
    FROM Yoneticiler y
    WHERE y.KullaniciAdi = p_kullanici AND y.SifreHash = p_sifre AND y.Aktif = TRUE;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT * FROM sp_yonetici_giris('admin', 'admin123');

