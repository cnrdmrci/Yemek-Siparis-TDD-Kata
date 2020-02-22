using System.Collections.Generic;

namespace YemekSiparisKata
{
    public interface IVeritabaniIletisimci
    {
        void VeritabaninaKaydet(SiparisBilgileri siparisBilgileri);
        void SiparisCekimBilgisiGuncelle(SiparisBilgileri siparisBilgileri, bool kartCekimiBasarilimi);
        List<SiparisBilgileri> CevapsizSiparisleriAl();
        void SiparisiIptalOlarakKaydet(SiparisBilgileri siparisBilgileri);
        void SiparisiOnaylandiOlarakKaydet(SiparisBilgileri siparisBilgileri);
    }
}