using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
                           WHERE FA.AntrenmanID = ?
                           ORDER BY F.Soyad, F.Ad";

            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId }
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
                           WHERE FA.FutbolcuID = ?
                           ORDER BY A.Tarih DESC";

            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@FutbolcuID", OleDbType.Integer) { Value = futbolcuId }
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
                        AntrenmanID = Convert.ToInt32(row["AntrenmanID"]),
                        Tarih = Convert.ToDateTime(row["Tarih"]),
                        Tur = row["Tur"].ToString()
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
                // Önce bu antrenmana ait tüm katılımları sil
                string deleteQuery = "DELETE FROM FutbolcuAntrenman WHERE AntrenmanID = ?";
                var deleteParam = new OleDbParameter[] { new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId } };
                DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteParam);

                // Sonra yeni katılımları ekle
                string insertQuery = "INSERT INTO FutbolcuAntrenman (FutbolcuID, AntrenmanID, Katilim, Performans) VALUES (?, ?, ?, ?)";
                
                foreach (var futbolcuKatilim in futbolcuKatilimlar)
                {
                    var parameters = new OleDbParameter[]
                    {
                        new OleDbParameter("@FutbolcuID", OleDbType.Integer) { Value = futbolcuKatilim.Key },
                        new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId },
                        new OleDbParameter("@Katilim", OleDbType.Boolean) { Value = futbolcuKatilim.Value },
                        new OleDbParameter("@Performans", OleDbType.Integer) { Value = 0 } // Başlangıçta performans değeri 0
                    };

                    DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Katılım kaydedilirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool KaydetPerformans(int futbolcuAntrenmanId, int performans, string notlar)
        {
            try
            {
                string updateQuery = "UPDATE FutbolcuAntrenman SET Performans = ?, Notlar = ? WHERE FutbolcuAntrenmanID = ?";
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Performans", OleDbType.Integer) { Value = performans },
                    new OleDbParameter("@Notlar", OleDbType.VarChar, 500) { Value = notlar ?? (object)DBNull.Value },
                    new OleDbParameter("@FutbolcuAntrenmanID", OleDbType.Integer) { Value = futbolcuAntrenmanId }
                };

                int result = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Performans kaydedilirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        private static FutbolcuAntrenman CreateFutbolcuAntrenmanFromDataRow(DataRow row)
        {
            FutbolcuAntrenman fa = new FutbolcuAntrenman
            {
                FutbolcuAntrenmanID = Convert.ToInt32(row["FutbolcuAntrenmanID"]),
                FutbolcuID = Convert.ToInt32(row["FutbolcuID"]),
                AntrenmanID = Convert.ToInt32(row["AntrenmanID"]),
                Katilim = Convert.ToBoolean(row["Katilim"]),
                Performans = Convert.ToInt32(row["Performans"])
            };

            // Notlar alanı null olabilir
            if (row["Notlar"] != DBNull.Value)
            {
                fa.Notlar = row["Notlar"].ToString();
            }

            // Eğer futbolcu bilgileri varsa
            if (row.Table.Columns.Contains("Ad") && row.Table.Columns.Contains("Soyad"))
            {
                fa.Futbolcu = new Futbolcu
                {
                    FutbolcuID = fa.FutbolcuID,
                    Ad = row["Ad"].ToString(),
                    Soyad = row["Soyad"].ToString()
                };

                if (row.Table.Columns.Contains("FormaNo"))
                {
                    fa.Futbolcu.FormaNo = Convert.ToInt32(row["FormaNo"]);
                }
            }

            return fa;
        }
    }
}