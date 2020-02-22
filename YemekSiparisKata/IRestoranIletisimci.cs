namespace YemekSiparisKata
{
    public interface IRestoranIletisimci
    {
        void SiparisBilgileriniGonder(SiparisBilgileri siparisBilgileri);
        void SiparisIptalIlet(SiparisBilgileri siparisBilgileri);
    }
}