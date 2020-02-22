namespace YemekSiparisKata
{
    public interface IBankaIletisimci
    {
        bool CekimYap(KrediKartiBilgileri krediKartiBilgileri, double tutar);
    }
}