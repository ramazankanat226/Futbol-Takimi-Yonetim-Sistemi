using System;

namespace FutbolTakimiYonetimSistemi.Models
{
    public class Futbolcu
    {
        public int FutbolcuID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
        public int Boy { get; set; }
        public int Kilo { get; set; }
        public string Pozisyon { get; set; }
        public int FormaNo { get; set; }
        public decimal Maas { get; set; }
        public DateTime SozlesmeBaslangic { get; set; }
        public DateTime SozlesmeBitis { get; set; }
        public string Uyruk { get; set; }
        public string Durumu { get; set; }
        public byte[] Resim { get; set; }
        public string Notlar { get; set; }

        public string TamAd => $"{Ad} {Soyad}";
        public int Yas => DateTime.Now.Year - DogumTarihi.Year - (DateTime.Now.DayOfYear < DogumTarihi.DayOfYear ? 1 : 0);
        public string KalanSozlesmeSuresi => $"{Math.Max(0, (SozlesmeBitis - DateTime.Now).Days / 30)} ay";
    }
} 