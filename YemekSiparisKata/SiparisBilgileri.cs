using System;

namespace YemekSiparisKata
{
    public class SiparisBilgileri
    {
        public SiparisOdemeTip OdemeTipi { get; set; }
        public KrediKartiBilgileri KrediKartiBilgileri { get; set; }
        public double ToplamTutar { get; set; }
        public DateTime SiparisTarihi { get; set; }
    }

    public class SiparisSonuc
    {
        public bool SiparisBasariliMi { get; set; }
    }

    public enum SiparisOdemeTip
    {
        KapidaNakit,
        KapidaKrediKarti,
        OnlineKrediKarti
    }

    public class KrediKartiBilgileri
    {
        public string KartNo { get; set; }
        public string AdiSoyadi { get; set; }
        public string GuvenlikKodu { get; set; }
    }
}