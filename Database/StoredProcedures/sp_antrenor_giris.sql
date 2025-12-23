-- ============================================
-- Stored Procedure: Antrenör Giriş Kontrolü
-- ============================================
-- Kullanıcı adı ve şifre kontrolü yapar, başarılı ise antrenör bilgilerini döndürür

CREATE OR REPLACE FUNCTION sp_antrenor_giris(p_kullanici VARCHAR, p_sifre VARCHAR)
RETURNS TABLE (
    AntrenorID INT, KullaniciAdi VARCHAR, Ad VARCHAR, 
    Soyad VARCHAR, Uzmanlik VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.AntrenorID, a.KullaniciAdi, a.Ad, a.Soyad, a.Uzmanlik
    FROM Antrenorler a
    WHERE a.KullaniciAdi = p_kullanici AND a.SifreHash = p_sifre AND a.Aktif = TRUE;
END;
$$ LANGUAGE plpgsql;

-- Örnek Kullanım:
-- SELECT * FROM sp_antrenor_giris('teknikdirektor', 'antrenor123');

