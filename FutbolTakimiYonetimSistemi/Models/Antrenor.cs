using System;

namespace FutbolTakimiYonetimSistemi.Models
{
    public class Antrenor
    {
        public int AntrenorID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Eposta { get; set; }
        public string TelefonNo { get; set; }
        public string Uzmanlik { get; set; }
        public string KullaniciTipi { get; set; }

        public string TamAd => $"{Ad} {Soyad}";
    }
}

