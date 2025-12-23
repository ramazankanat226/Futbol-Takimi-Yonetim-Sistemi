using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class FutbolcuAntrenmanService
    {
        public static List<FutbolcuAntrenman> GetByAntrenmanId(int antrenmanId)
        {
            string query = @"SELECT FA.*, F.Ad, F.Soyad, F.FormaNo
                           FROM FutbolcuAntrenman FA
                           INNER JOIN Futbolcular F ON FA.FutbolcuID = F.FutbolcuID
                           WHERE FA.AntrenmanID = @AntrenmanID
                           ORDER BY F.Soyad, F.Ad";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@AntrenmanID", antrenmanId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            List<FutbolcuAntrenman> futbolcuAntrenmanlar = new List<FutbolcuAntrenman>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    futbolcuAntrenmanlar.Add(CreateFutbolcuAntrenmanFromDataRow(row));
                }
            }

            return futbolcuAntrenmanlar;
        }

        public static List<FutbolcuAntrenman> GetByFutbolcuId(int futbolcuId)
        {
            string query = @"SELECT FA.*, A.Tarih, A.Tur
                           FROM FutbolcuAntrenman FA
                           INNER JOIN Antrenmanlar A ON FA.AntrenmanID = A.AntrenmanID
                           WHERE FA.FutbolcuID = @FutbolcuID
                           ORDER BY A.Tarih DESC";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@FutbolcuID", futbolcuId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            List<FutbolcuAntrenman> futbolcuAntrenmanlar = new List<FutbolcuAntrenman>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    FutbolcuAntrenman fa = CreateFutbolcuAntrenmanFromDataRow(row);
                    fa.Antrenman = new Antrenman
                    {
                        AntrenmanID = Convert.ToInt32(row["antrenmanid"]),
                        Tarih = Convert.ToDateTime(row["tarih"]),
                        Tur = row["tur"].ToString()
                    };
                    futbolcuAntrenmanlar.Add(fa);
                }
            }

            return futbolcuAntrenmanlar;
        }

        public static bool KaydetKatilim(int antrenmanId, Dictionary<int, bool> futbolcuKatilimlar)
        {
            try
            {
                // PostgreSQL'de function SELECT ile çağrılır
                string query = "SELECT sp_katilim_ekle(@p_futbolcu_id, @p_antrenman_id, @p_katilim, @p_performans, @p_notlar)";
                
                foreach (var futbolcuKatilim in futbolcuKatilimlar)
                {
                    var parameters = new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@p_futbolcu_id", futbolcuKatilim.Key),
                        new NpgsqlParameter("@p_antrenman_id", antrenmanId),
                        new NpgsqlParameter("@p_katilim", futbolcuKatilim.Value),
                        new NpgsqlParameter("@p_performans", DBNull.Value),
                        new NpgsqlParameter("@p_notlar", DBNull.Value)
                    };

                    DatabaseHelper.ExecuteScalar(query, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Katılım kaydedilirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool KaydetPerformans(int futbolcuAntrenmanId, int performans, string notlar)
        {
            try
            {
                string updateQuery = "UPDATE FutbolcuAntrenman SET Performans = @Performans, Notlar = @Notlar WHERE FutbolcuAntrenmanID = @FutbolcuAntrenmanID";
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@Performans", performans),
                    new NpgsqlParameter("@Notlar", (object)notlar ?? DBNull.Value),
                    new NpgsqlParameter("@FutbolcuAntrenmanID", futbolcuAntrenmanId)
                };

                int result = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Performans kaydedilirken hata: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        private static FutbolcuAntrenman CreateFutbolcuAntrenmanFromDataRow(DataRow row)
        {
            FutbolcuAntrenman fa = new FutbolcuAntrenman
            {
                FutbolcuAntrenmanID = Convert.ToInt32(row["futbolcuantrenmanid"]),
                FutbolcuID = Convert.ToInt32(row["futbolcuid"]),
                AntrenmanID = Convert.ToInt32(row["antrenmanid"]),
                Katilim = Convert.ToBoolean(row["katilim"]),
                Performans = row["performans"] != DBNull.Value ? Convert.ToInt32(row["performans"]) : 0
            };

            if (row["notlar"] != DBNull.Value)
            {
                fa.Notlar = row["notlar"].ToString();
            }

            // Futbolcu bilgileri varsa
            if (row.Table.Columns.Contains("ad") && row.Table.Columns.Contains("soyad"))
            {
                fa.Futbolcu = new Futbolcu
                {
                    FutbolcuID = fa.FutbolcuID,
                    Ad = row["ad"].ToString(),
                    Soyad = row["soyad"].ToString()
                };

                if (row.Table.Columns.Contains("formano"))
                {
                    fa.Futbolcu.FormaNo = Convert.ToInt32(row["formano"]);
                }
            }

            return fa;
        }
    }
}
