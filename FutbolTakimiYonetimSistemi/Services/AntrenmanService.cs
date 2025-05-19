using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
                        try
                        {
                            Antrenman antrenman = CreateAntrenmanFromDataRow(row);
                            if (antrenman != null)
                            {
                                antrenmanlar.Add(antrenman);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show($"Antrenman verisi işlenirken hata: {ex.Message}", "Veri Hatası", 
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }

                return antrenmanlar;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenmanlar getirilirken hata oluştu: {ex.Message}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return new List<Antrenman>(); // Boş liste döndür
            }
        }

        public static Antrenman GetById(int antrenmanId)
        {
            try
            {
                string query = "SELECT * FROM Antrenmanlar WHERE AntrenmanID = ?";
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId }
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
                System.Windows.Forms.MessageBox.Show($"Antrenman bilgisi alınırken hata oluştu: {ex.Message}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool EkleAntrenman(Antrenman antrenman)
        {
            if (antrenman == null)
            {
                System.Windows.Forms.MessageBox.Show("Antrenman bilgileri geçerli değil.", 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

            try
            {
                // Tarih parametrelerini doğru formata dönüştürme
                DateTime tarih = antrenman.Tarih.Date; // Sadece tarih kısmını al

                string query = @"INSERT INTO Antrenmanlar (Tarih, BaslangicSaati, BitisSaati, Tur, Notlar) 
                            VALUES (?, ?, ?, ?, ?)";

                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Tarih", OleDbType.Date) { Value = tarih },
                    new OleDbParameter("@BaslangicSaati", OleDbType.Date) { Value = antrenman.BaslangicSaati },
                    new OleDbParameter("@BitisSaati", OleDbType.Date) { Value = antrenman.BitisSaati },
                    new OleDbParameter("@Tur", OleDbType.VarChar, 50) { Value = antrenman.Tur ?? "" },
                    new OleDbParameter("@Notlar", OleDbType.VarChar, 1000) { Value = antrenman.Notlar ?? (object)DBNull.Value }
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (OleDbException dbEx)
            {
                System.Windows.Forms.MessageBox.Show($"Veritabanı hatası: {dbEx.Message}\nHata Kodu: {dbEx.ErrorCode}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Antrenman eklenirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool GuncelleAntrenman(Antrenman antrenman)
        {
            if (antrenman == null)
            {
                System.Windows.Forms.MessageBox.Show("Antrenman bilgileri geçerli değil.", 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

            try
            {
                // Tarih parametrelerini doğru formata dönüştürme
                DateTime tarih = antrenman.Tarih.Date; // Sadece tarih kısmını al

                string query = @"UPDATE Antrenmanlar SET Tarih = ?, BaslangicSaati = ?, BitisSaati = ?, 
                            Tur = ?, Notlar = ? WHERE AntrenmanID = ?";

                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Tarih", OleDbType.Date) { Value = tarih },
                    new OleDbParameter("@BaslangicSaati", OleDbType.Date) { Value = antrenman.BaslangicSaati },
                    new OleDbParameter("@BitisSaati", OleDbType.Date) { Value = antrenman.BitisSaati },
                    new OleDbParameter("@Tur", OleDbType.VarChar, 50) { Value = antrenman.Tur ?? "" },
                    new OleDbParameter("@Notlar", OleDbType.VarChar, 1000) { Value = antrenman.Notlar ?? (object)DBNull.Value },
                    new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenman.AntrenmanID }
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (OleDbException dbEx)
            {
                System.Windows.Forms.MessageBox.Show($"Veritabanı hatası: {dbEx.Message}\nHata Kodu: {dbEx.ErrorCode}", 
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Antrenman güncellenirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool SilAntrenman(int antrenmanId)
        {
            try
            {
                // İlk olarak bu antrenmana ait tüm katılım kayıtlarını silmemiz gerekiyor
                string deleteKatilimQuery = "DELETE FROM FutbolcuAntrenman WHERE AntrenmanID = ?";
                var katilimParameters = new OleDbParameter[]
                {
                    new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId }
                };

                // Katılım kayıtlarını sil
                DatabaseHelper.ExecuteNonQuery(deleteKatilimQuery, katilimParameters);

                // Sonra antrenmanı sil
                string deleteAntrenmanQuery = "DELETE FROM Antrenmanlar WHERE AntrenmanID = ?";
                var antrenmanParameters = new OleDbParameter[]
                {
                    new OleDbParameter("@AntrenmanID", OleDbType.Integer) { Value = antrenmanId }
                };

                int result = DatabaseHelper.ExecuteNonQuery(deleteAntrenmanQuery, antrenmanParameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenman silinirken hata oluştu: {ex.Message}",
                    "Veritabanı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static List<Antrenman> AntrenmanAra(string aramaMetni)
        {
            try
            {
                string query = "SELECT * FROM Antrenmanlar WHERE Tur LIKE ? OR Notlar LIKE ? ORDER BY Tarih DESC";
                string aramaParametresi = "%" + aramaMetni + "%";
                
                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Tur", OleDbType.VarChar, 50) { Value = aramaParametresi },
                    new OleDbParameter("@Notlar", OleDbType.VarChar, 1000) { Value = aramaParametresi }
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
                List<Antrenman> antrenmanlar = new List<Antrenman>();

                if (result != null)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        try
                        {
                            Antrenman antrenman = CreateAntrenmanFromDataRow(row);
                            if (antrenman != null)
                            {
                                antrenmanlar.Add(antrenman);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show($"Antrenman arama sonucu işlenirken hata: {ex.Message}", 
                                "Veri Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }

                return antrenmanlar;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenman arama sırasında hata oluştu: {ex.Message}", 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return new List<Antrenman>(); // Boş liste döndür
            }
        }

        private static Antrenman CreateAntrenmanFromDataRow(DataRow row)
        {
            try
            {
                if (row == null) return null;
                
                Antrenman antrenman = new Antrenman
                {
                    AntrenmanID = Convert.ToInt32(row["AntrenmanID"]),
                    Tarih = Convert.ToDateTime(row["Tarih"]),
                    BaslangicSaati = Convert.ToDateTime(row["BaslangicSaati"]),
                    BitisSaati = Convert.ToDateTime(row["BitisSaati"]),
                    Tur = row["Tur"]?.ToString() ?? ""
                };

                // Notlar alanı null olabilir
                if (row["Notlar"] != DBNull.Value)
                {
                    antrenman.Notlar = row["Notlar"].ToString();
                }

                return antrenman;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Antrenman verisi dönüştürülürken hata: {ex.Message}", 
                    "Veri Dönüşüm Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }
    }
} 