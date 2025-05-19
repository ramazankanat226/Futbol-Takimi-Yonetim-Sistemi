using System;

namespace FutbolTakimiYonetimSistemi.Models
{
    public class Yonetici
    {
        public int YoneticiID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Eposta { get; set; }
        public string TelefonNo { get; set; }

        public string TamAd => $"{Ad} {Soyad}";
    }
} 