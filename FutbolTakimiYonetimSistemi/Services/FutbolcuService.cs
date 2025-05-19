using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class FutbolcuService
    {
        public static List<Futbolcu> GetAllFutbolcular()
        {
            string query = "SELECT * FROM Futbolcular ORDER BY Soyad, Ad";
            DataTable result = DatabaseHelper.ExecuteQuery(query);
            List<Futbolcu> futbolcular = new List<Futbolcu>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    futbolcular.Add(CreateFutbolcuFromDataRow(row));
                }
            }

            return futbolcular;
        }

        public static Futbolcu GetById(int futbolcuId)
        {
            string query = "SELECT * FROM Futbolcular WHERE FutbolcuID = ?";
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@FutbolcuID", futbolcuId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                return CreateFutbolcuFromDataRow(result.Rows[0]);
            }

            return null;
        }

        public static bool EkleFutbolcu(Futbolcu futbolcu)
        {
            try
            {
                // Tarih parametrelerini doğru formata dönüştürme
                DateTime dogumTarihi = futbolcu.DogumTarihi.Date; // Sadece tarih kısmını al, saat kısmını temizle
                DateTime sozlesmeBaslangic = futbolcu.SozlesmeBaslangic.Date;
                DateTime sozlesmeBitis = futbolcu.SozlesmeBitis.Date;

                string query = @"INSERT INTO Futbolcular (Ad, Soyad, DogumTarihi, Boy, Kilo, Pozisyon, 
                            FormaNo, Maas, SozlesmeBaslangic, SozlesmeBitis, Uyruk, Durumu) 
                            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Ad", OleDbType.VarChar) { Value = futbolcu.Ad },
                    new OleDbParameter("@Soyad", OleDbType.VarChar) { Value = futbolcu.Soyad },
                    new OleDbParameter("@DogumTarihi", OleDbType.Date) { Value = dogumTarihi },
                    new OleDbParameter("@Boy", OleDbType.Integer) { Value = futbolcu.Boy },
                    new OleDbParameter("@Kilo", OleDbType.Integer) { Value = futbolcu.Kilo },
                    new OleDbParameter("@Pozisyon", OleDbType.VarChar) { Value = futbolcu.Pozisyon },
                    new OleDbParameter("@FormaNo", OleDbType.Integer) { Value = futbolcu.FormaNo },
                    new OleDbParameter("@Maas", OleDbType.Currency) { Value = futbolcu.Maas },
                    new OleDbParameter("@SozlesmeBaslangic", OleDbType.Date) { Value = sozlesmeBaslangic },
                    new OleDbParameter("@SozlesmeBitis", OleDbType.Date) { Value = sozlesmeBitis },
                    new OleDbParameter("@Uyruk", OleDbType.VarChar) { Value = futbolcu.Uyruk },
                    new OleDbParameter("@Durumu", OleDbType.VarChar) { Value = futbolcu.Durumu }
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Futbolcu eklenirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool GuncelleFutbolcu(Futbolcu futbolcu)
        {
            try
            {
                // Tarih parametrelerini doğru formata dönüştürme
                DateTime dogumTarihi = futbolcu.DogumTarihi.Date; // Sadece tarih kısmını al, saat kısmını temizle
                DateTime sozlesmeBaslangic = futbolcu.SozlesmeBaslangic.Date;
                DateTime sozlesmeBitis = futbolcu.SozlesmeBitis.Date;

                string query = @"UPDATE Futbolcular SET Ad = ?, Soyad = ?, DogumTarihi = ?, Boy = ?, 
                            Kilo = ?, Pozisyon = ?, FormaNo = ?, Maas = ?, SozlesmeBaslangic = ?, 
                            SozlesmeBitis = ?, Uyruk = ?, Durumu = ? WHERE FutbolcuID = ?";

                var parameters = new OleDbParameter[]
                {
                    new OleDbParameter("@Ad", OleDbType.VarChar) { Value = futbolcu.Ad },
                    new OleDbParameter("@Soyad", OleDbType.VarChar) { Value = futbolcu.Soyad },
                    new OleDbParameter("@DogumTarihi", OleDbType.Date) { Value = dogumTarihi },
                    new OleDbParameter("@Boy", OleDbType.Integer) { Value = futbolcu.Boy },
                    new OleDbParameter("@Kilo", OleDbType.Integer) { Value = futbolcu.Kilo },
                    new OleDbParameter("@Pozisyon", OleDbType.VarChar) { Value = futbolcu.Pozisyon },
                    new OleDbParameter("@FormaNo", OleDbType.Integer) { Value = futbolcu.FormaNo },
                    new OleDbParameter("@Maas", OleDbType.Currency) { Value = futbolcu.Maas },
                    new OleDbParameter("@SozlesmeBaslangic", OleDbType.Date) { Value = sozlesmeBaslangic },
                    new OleDbParameter("@SozlesmeBitis", OleDbType.Date) { Value = sozlesmeBitis },
                    new OleDbParameter("@Uyruk", OleDbType.VarChar) { Value = futbolcu.Uyruk },
                    new OleDbParameter("@Durumu", OleDbType.VarChar) { Value = futbolcu.Durumu },
                    new OleDbParameter("@FutbolcuID", OleDbType.Integer) { Value = futbolcu.FutbolcuID }
                };

                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Futbolcu güncellenirken hata oluştu: " + ex.Message, 
                    "Hata", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool SilFutbolcu(int futbolcuId)
        {
            string query = "DELETE FROM Futbolcular WHERE FutbolcuID = ?";
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@FutbolcuID", futbolcuId)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public static List<Futbolcu> FutbolcuAra(string aramaMetni)
        {
            string query = "SELECT * FROM Futbolcular WHERE Ad LIKE ? OR Soyad LIKE ? OR Pozisyon LIKE ? OR Uyruk LIKE ? ORDER BY Soyad, Ad";
            string aramaParametresi = "%" + aramaMetni + "%";
            
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@Ad", aramaParametresi),
                new OleDbParameter("@Soyad", aramaParametresi),
                new OleDbParameter("@Pozisyon", aramaParametresi),
                new OleDbParameter("@Uyruk", aramaParametresi)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);
            List<Futbolcu> futbolcular = new List<Futbolcu>();

            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    futbolcular.Add(CreateFutbolcuFromDataRow(row));
                }
            }

            return futbolcular;
        }

        private static Futbolcu CreateFutbolcuFromDataRow(DataRow row)
        {
            Futbolcu futbolcu = new Futbolcu
            {
                FutbolcuID = Convert.ToInt32(row["FutbolcuID"]),
                Ad = row["Ad"].ToString(),
                Soyad = row["Soyad"].ToString(),
                DogumTarihi = Convert.ToDateTime(row["DogumTarihi"]),
                Boy = Convert.ToInt32(row["Boy"]),
                Kilo = Convert.ToInt32(row["Kilo"]),
                Pozisyon = row["Pozisyon"].ToString(),
                FormaNo = Convert.ToInt32(row["FormaNo"]),
                Maas = Convert.ToDecimal(row["Maas"]),
                SozlesmeBaslangic = Convert.ToDateTime(row["SozlesmeBaslangic"]),
                SozlesmeBitis = Convert.ToDateTime(row["SozlesmeBitis"]),
                Uyruk = row["Uyruk"].ToString(),
                Durumu = row["Durumu"].ToString()
            };

            // Resim alanı null olabilir
            if (row["Resim"] != DBNull.Value)
            {
                futbolcu.Resim = (byte[])row["Resim"];
            }

            return futbolcu;
        }
    }
} 