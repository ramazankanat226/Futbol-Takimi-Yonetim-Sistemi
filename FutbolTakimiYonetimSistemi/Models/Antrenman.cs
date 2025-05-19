using System;

namespace FutbolTakimiYonetimSistemi.Models
{
    public class Antrenman
    {
        public int AntrenmanID { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime BaslangicSaati { get; set; }
        public DateTime BitisSaati { get; set; }
        public string Tur { get; set; }
        public string Notlar { get; set; }

        public string TurSecenek
        {
            get
            {
                return Tur ?? "Belirtilmemiş";
            }
        }

        public string AntrenmanBilgisi
        {
            get
            {
                return $"{Tarih.ToShortDateString()} - {TurSecenek}";
            }
        }
    }
} 