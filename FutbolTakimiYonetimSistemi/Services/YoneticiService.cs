using System;
using System.Data;
using Npgsql;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Utils;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class YoneticiService
    {
       
        public static Yonetici Giris(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                return null;
            }

            try
            {
                
                string query = "SELECT * FROM Yoneticiler WHERE KullaniciAdi = @KullaniciAdi AND Aktif = TRUE";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@KullaniciAdi", kullaniciAdi)
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                
                if (result == null)
                {
                    System.Windows.Forms.MessageBox.Show("Veritabanı bağlantı hatası! Giriş yapılamıyor.", "Hata", 
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    string storedHash = row["sifrehash"].ToString();

                    // Şifre kontrolü (otomatik hash güncellemesi ile)
                    var (isValid, newHash) = PasswordHelper.MigrateOldPassword(sifre, storedHash);

                    if (isValid)
                    {
                        // Eğer eski düz metin şifre ise, hash'lenmiş versiyonu kaydet
                        if (newHash != storedHash)
                        {
                            UpdatePasswordHash(Convert.ToInt32(row["yoneticiid"]), newHash);
                        }

                        Yonetici yonetici = new Yonetici
                        {
                            YoneticiID = Convert.ToInt32(row["yoneticiid"]),
                            KullaniciAdi = row["kullaniciadi"].ToString(),
                            Ad = row["ad"].ToString(),
                            Soyad = row["soyad"].ToString(),
                            KullaniciTipi = row["kullanicitipi"].ToString(),
                            Eposta = row["eposta"] != DBNull.Value ? row["eposta"].ToString() : null,
                            TelefonNo = row["telefonno"] != DBNull.Value ? row["telefonno"].ToString() : null
                        };

                        // Session'a kaydet
                        SessionManager.Instance.LoginYonetici(yonetici);

                        return yonetici;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Giriş hatası: {ex.Message}", "Hata", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Şifre hash'ini günceller (güvenlik için)
        /// </summary>
        private static void UpdatePasswordHash(int yoneticiId, string newHash)
        {
            try
            {
                string query = "UPDATE Yoneticiler SET SifreHash = @SifreHash WHERE YoneticiID = @YoneticiID";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@SifreHash", newHash),
                    new NpgsqlParameter("@YoneticiID", yoneticiId)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch
            {
                // Sessizce başarısız ol - giriş engellenmemeli
            }
        }

        public static Yonetici GetById(int yoneticiId)
        {
            string query = "SELECT * FROM Yoneticiler WHERE YoneticiID = @YoneticiID";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@YoneticiID", yoneticiId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Yonetici
                {
                    YoneticiID = Convert.ToInt32(row["yoneticiid"]),
                    KullaniciAdi = row["kullaniciadi"].ToString(),
                    Ad = row["ad"].ToString(),
                    Soyad = row["soyad"].ToString(),
                    Eposta = row["eposta"] != DBNull.Value ? row["eposta"].ToString() : null,
                    TelefonNo = row["telefonno"] != DBNull.Value ? row["telefonno"].ToString() : null,
                    KullaniciTipi = row["kullanicitipi"].ToString()
                };
            }

            return null;
        }

        /// <summary>
        /// Yeni yönetici ekler (şifre hash'lenerek)
        /// </summary>
        public static bool EkleYonetici(Yonetici yonetici, string sifre)
        {
            try
            {
                // Şifreyi hash'le
                string sifreHash = PasswordHelper.HashPassword(sifre);

                string query = @"INSERT INTO Yoneticiler (KullaniciAdi, SifreHash, Ad, Soyad, Eposta, TelefonNo, KullaniciTipi, Aktif)
                                VALUES (@KullaniciAdi, @SifreHash, @Ad, @Soyad, @Eposta, @TelefonNo, @KullaniciTipi, TRUE)";

                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@KullaniciAdi", yonetici.KullaniciAdi),
                    new NpgsqlParameter("@SifreHash", sifreHash),
                    new NpgsqlParameter("@Ad", yonetici.Ad),
                    new NpgsqlParameter("@Soyad", yonetici.Soyad),
                    new NpgsqlParameter("@Eposta", (object)yonetici.Eposta ?? DBNull.Value),
                    new NpgsqlParameter("@TelefonNo", (object)yonetici.TelefonNo ?? DBNull.Value),
                    new NpgsqlParameter("@KullaniciTipi", yonetici.KullaniciTipi ?? "Yonetici")
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Yönetici eklenirken hata: {ex.Message}", "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
