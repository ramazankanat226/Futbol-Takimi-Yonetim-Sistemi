using System;
using System.Data;
using Npgsql;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;
using FutbolTakimiYonetimSistemi.Utils;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class AntrenorService
    {
        /// <summary>
        /// Antrenör giriş kontrolü - BCrypt şifre doğrulama ile
        /// </summary>
        public static Antrenor Giris(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                return null;
            }

            try
            {
                // Kullanıcıyı getir
                string query = "SELECT * FROM Antrenorler WHERE KullaniciAdi = @KullaniciAdi AND Aktif = TRUE";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@KullaniciAdi", kullaniciAdi)
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                // GÜVENLİK: Eğer veritabanı hatası olduysa giriş yapılmasın
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
                            UpdatePasswordHash(Convert.ToInt32(row["antrenorid"]), newHash);
                        }

                        Antrenor antrenor = new Antrenor
                        {
                            AntrenorID = Convert.ToInt32(row["antrenorid"]),
                            KullaniciAdi = row["kullaniciadi"].ToString(),
                            Ad = row["ad"].ToString(),
                            Soyad = row["soyad"].ToString(),
                            Uzmanlik = row["uzmanlik"] != DBNull.Value ? row["uzmanlik"].ToString() : null,
                            KullaniciTipi = row["kullanicitipi"] != DBNull.Value ? row["kullanicitipi"].ToString() : "Antrenor",
                            Eposta = row["eposta"] != DBNull.Value ? row["eposta"].ToString() : null,
                            TelefonNo = row["telefonno"] != DBNull.Value ? row["telefonno"].ToString() : null
                        };

                        // Session'a kaydet
                        SessionManager.Instance.LoginAntrenor(antrenor);

                        return antrenor;
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
        private static void UpdatePasswordHash(int antrenorId, string newHash)
        {
            try
            {
                string query = "UPDATE Antrenorler SET SifreHash = @SifreHash WHERE AntrenorID = @AntrenorID";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@SifreHash", newHash),
                    new NpgsqlParameter("@AntrenorID", antrenorId)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch
            {
                // Sessizce başarısız ol
            }
        }
    }
}
