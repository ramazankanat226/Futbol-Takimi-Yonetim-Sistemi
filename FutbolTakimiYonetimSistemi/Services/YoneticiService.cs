using System;
using System.Data;
using System.Data.OleDb;
using FutbolTakimiYonetimSistemi.Data;
using FutbolTakimiYonetimSistemi.Models;

namespace FutbolTakimiYonetimSistemi.Services
{
    public class YoneticiService
    {
        public static Yonetici Giris(string kullaniciAdi, string sifre)
        {
            string query = "SELECT * FROM Yoneticiler WHERE KullaniciAdi = ? AND Sifre = ?";
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@KullaniciAdi", kullaniciAdi),
                new OleDbParameter("@Sifre", sifre)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Yonetici
                {
                    YoneticiID = Convert.ToInt32(row["YoneticiID"]),
                    KullaniciAdi = row["KullaniciAdi"].ToString(),
                    Sifre = row["Sifre"].ToString(),
                    Ad = row["Ad"].ToString(),
                    Soyad = row["Soyad"].ToString(),
                    Eposta = row["Eposta"].ToString(),
                    TelefonNo = row["TelefonNo"].ToString()
                };
            }

            return null;
        }

        public static Yonetici GetById(int yoneticiId)
        {
            string query = "SELECT * FROM Yoneticiler WHERE YoneticiID = ?";
            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("@YoneticiID", yoneticiId)
            };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Yonetici
                {
                    YoneticiID = Convert.ToInt32(row["YoneticiID"]),
                    KullaniciAdi = row["KullaniciAdi"].ToString(),
                    Sifre = row["Sifre"].ToString(),
                    Ad = row["Ad"].ToString(),
                    Soyad = row["Soyad"].ToString(),
                    Eposta = row["Eposta"].ToString(),
                    TelefonNo = row["TelefonNo"].ToString()
                };
            }

            return null;
        }
    }
} 