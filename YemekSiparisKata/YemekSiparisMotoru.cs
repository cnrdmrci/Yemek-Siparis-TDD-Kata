using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YemekSiparisKata
{
    public class YemekSiparisMotoru
    {
        private IRestoranIletisimci _restoranIletisimci;
        private IBankaIletisimci _bankaIletisimci;
        private IVeritabaniIletisimci _veritabaniIletisimci;
        private ICagriMerkeziIletisimci _cagriMerkeziIletisimci;
        public YemekSiparisMotoru(IRestoranIletisimci restoranIletisimci, IBankaIletisimci bankaIletisimci, IVeritabaniIletisimci veritabaniIletisimci, ICagriMerkeziIletisimci cagriMerkeziIletisimci)
        {
            _restoranIletisimci = restoranIletisimci;
            _bankaIletisimci = bankaIletisimci;
            _veritabaniIletisimci = veritabaniIletisimci;
            _cagriMerkeziIletisimci = cagriMerkeziIletisimci;
        }

        public SiparisSonuc SiparisVer(SiparisBilgileri siparisBilgileri)
        {
            _veritabaniIletisimci.VeritabaninaKaydet(siparisBilgileri);

            if (siparisBilgileri.OdemeTipi == SiparisOdemeTip.OnlineKrediKarti)
            {
                bool cekimSonucu = _bankaIletisimci.CekimYap(siparisBilgileri.KrediKartiBilgileri, siparisBilgileri.ToplamTutar);
                _veritabaniIletisimci.SiparisCekimBilgisiGuncelle(siparisBilgileri,cekimSonucu);
                if (!cekimSonucu)
                    return new SiparisSonuc {SiparisBasariliMi = false};
            }

            _restoranIletisimci.SiparisBilgileriniGonder(siparisBilgileri);

            return new SiparisSonuc {SiparisBasariliMi = true};
        }

        public void RestoranCevabiniIsle(SiparisBilgileri siparisBilgileri, bool siparisOnaylandiMi)
        {
            if(siparisOnaylandiMi)
                _veritabaniIletisimci.SiparisiOnaylandiOlarakKaydet(siparisBilgileri);
            else
            {
                _veritabaniIletisimci.SiparisiIptalOlarakKaydet(siparisBilgileri);
                _cagriMerkeziIletisimci.SiparisIptalBilgisiIlet(siparisBilgileri);
            }
        }

        public void BelliBirSuredirCevapAlinamayanSiparisleriIptalEt()
        {
            var besDakikadanFazlaSuredirBekleyenSiparisler = _veritabaniIletisimci.CevapsizSiparisleriAl().Where(siparis => DateTime.Now.Subtract(siparis.SiparisTarihi).TotalMinutes >= 5);
            foreach (SiparisBilgileri siparis in besDakikadanFazlaSuredirBekleyenSiparisler)
            {
                _restoranIletisimci.SiparisIptalIlet(siparis);
                _veritabaniIletisimci.SiparisiIptalOlarakKaydet(siparis);
                _cagriMerkeziIletisimci.SiparisIptalBilgisiIlet(siparis);
            }
        }
    }
}
