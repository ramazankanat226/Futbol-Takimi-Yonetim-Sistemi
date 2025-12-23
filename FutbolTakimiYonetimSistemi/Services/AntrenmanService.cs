using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class AntrenmanService
    {
        public static List<Antrenman> GetAllAntrenmanlar()
        {
            try
            {
                string query = "SELECT * FROM Antrenmanlar ORDER BY Tarih DESC, BaslangicSaati";
                DataTable result = DatabaseHelper.ExecuteQuery(query);
                List<Antrenman> antrenmanlar = new List<Antrenman>();

                if (result != null)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        antrenmanlar.Add(CreateAntrenmanFromDataRow(row));
                    }
                }

                return antrenmanlar;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenmanlar getirilirken hata: {ex.Message}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return new List<Antrenman>();
            }
        }

        public static Antrenman GetById(int antrenmanId)
        {
            try
            {
                string query = "SELECT * FROM Antrenmanlar WHERE AntrenmanID = @AntrenmanID";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@AntrenmanID", antrenmanId)
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    return CreateAntrenmanFromDataRow(result.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenman bilgisi alınırken hata: {ex.Message}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool EkleAntrenman(Antrenman antrenman)
        {
            try
            {
                // Session'dan antrenör ID'sini al
                var session = FutbolTakimiYonetimSistemi.Utils.SessionManager.Instance;
                
                // Sadece antrenörler antrenman ekleyebilir
                if (!session.IsAuthenticated)
                {
                    throw new Exceptions.UnauthorizedException("Lütfen önce giriş yapın!");
                }

                int antrenorId;
                if (session.KullaniciTipi == "Antrenor" || session.KullaniciTipi == "TeknikDirektor")
                {
                    antrenorId = session.KullaniciID;
                }
                else
                {
                    // Yönetici ise varsayılan antrenör ID
                    antrenorId = 1;
                }

                // PostgreSQL'de function SELECT ile çağrılır
                string query = "SELECT sp_antrenman_ekle(@p_tarih, @p_baslangic, @p_bitis, @p_tur, @p_notlar, @p_antrenor_id)";
                
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@p_tarih", NpgsqlTypes.NpgsqlDbType.Date){ Value = antrenman.Tarih.Date },
                    new NpgsqlParameter("@p_baslangic", NpgsqlTypes.NpgsqlDbType.Time){ Value = antrenman.BaslangicSaati.TimeOfDay },
                    new NpgsqlParameter("@p_bitis", NpgsqlTypes.NpgsqlDbType.Time){ Value = antrenman.BitisSaati.TimeOfDay },
                    new NpgsqlParameter("@p_tur", antrenman.Tur ?? ""),
                    new NpgsqlParameter("@p_notlar", (object)antrenman.Notlar ?? DBNull.Value),
                    new NpgsqlParameter("@p_antrenor_id", antrenorId)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exceptions.BusinessException)
            {
                throw; // Business exception'ları yukarı fırlat
            }
            catch (Exception ex)
            {
                throw new Exceptions.DatabaseException("Antrenman eklenirken hata oluştu", ex);
            }
        }

        public static bool GuncelleAntrenman(Antrenman antrenman)
        {
            try
            {
                string query = @"UPDATE Antrenmanlar SET Tarih = @Tarih, BaslangicSaati = @BaslangicSaati, 
                                BitisSaati = @BitisSaati, Tur = @Tur, Notlar = @Notlar 
                                WHERE AntrenmanID = @AntrenmanID";

                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@Tarih", antrenman.Tarih.Date),
                    new NpgsqlParameter("@BaslangicSaati", antrenman.BaslangicSaati.TimeOfDay),
                    new NpgsqlParameter("@BitisSaati", antrenman.BitisSaati.TimeOfDay),
                    new NpgsqlParameter("@Tur", antrenman.Tur ?? ""),
                    new NpgsqlParameter("@Notlar", (object)antrenman.Notlar ?? DBNull.Value),
                    new NpgsqlParameter("@AntrenmanID", antrenman.AntrenmanID)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Antrenman güncellenirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool SilAntrenman(int antrenmanId)
        {
            try
            {
                // Cascade delete ile katılım kayıtları otomatik silinecek
                string query = "DELETE FROM Antrenmanlar WHERE AntrenmanID = @AntrenmanID";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@AntrenmanID", antrenmanId)
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenman silinirken hata: {ex.Message}",
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        private static Antrenman CreateAntrenmanFromDataRow(DataRow row)
        {
            Antrenman antrenman = new Antrenman
            {
                AntrenmanID = Convert.ToInt32(row["antrenmanid"]),
                Tarih = Convert.ToDateTime(row["tarih"]),
                BaslangicSaati = DateTime.Today.Add((TimeSpan)row["baslangicsaati"]),
                BitisSaati = DateTime.Today.Add((TimeSpan)row["bitissaati"]),
                Tur = row["tur"]?.ToString() ?? ""
            };

            if (row["notlar"] != DBNull.Value)
            {
                antrenman.Notlar = row["notlar"].ToString();
            }

            return antrenman;
        }
    }
}
