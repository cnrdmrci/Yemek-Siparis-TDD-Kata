using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;

namespace YemekSiparisKata.NunitTest
{
    [TestFixture]
    public class YemekSiparisMotoruTest
    {
        private Mock<IRestoranIletisimci> _restoranIletisimci;
        private Mock<IBankaIletisimci> _bankaIletisimci;
        private Mock<IVeritabaniIletisimci> _veritabaniIletisimci;
        private Mock<ICagriMerkeziIletisimci> _cagriMerkeziIletisimci;
        private YemekSiparisMotoru _yemekSiparisMotoru;
        private SiparisBilgileri _siparisBilgiOnlineOdemeli;
        private SiparisBilgileri _siparisBilgiOnlineOdemesiz;

        [SetUp]
        public void Init()
        {
            _restoranIletisimci = new Mock<IRestoranIletisimci>();
            _bankaIletisimci = new Mock<IBankaIletisimci>();
            _veritabaniIletisimci = new Mock<IVeritabaniIletisimci>();
            _cagriMerkeziIletisimci = new Mock<ICagriMerkeziIletisimci>();
            _yemekSiparisMotoru = new YemekSiparisMotoru(_restoranIletisimci.Object,_bankaIletisimci.Object,_veritabaniIletisimci.Object,_cagriMerkeziIletisimci.Object);
            
            _siparisBilgiOnlineOdemeli = new SiparisBilgileri
            {
                KrediKartiBilgileri = new KrediKartiBilgileri(),
                OdemeTipi = SiparisOdemeTip.OnlineKrediKarti,
                SiparisTarihi = DateTime.Now,
                ToplamTutar = 100
            };

            _siparisBilgiOnlineOdemesiz = new SiparisBilgileri
            {
                KrediKartiBilgileri = new KrediKartiBilgileri(),
                OdemeTipi = SiparisOdemeTip.KapidaKrediKarti,
                SiparisTarihi = DateTime.Now,
                ToplamTutar = 100
            };
        }

        [TearDown]
        public void Cleanup()
        { /* ... */ }

        [Test]
        public void SiparisGeldiginde_IlgiliRestoranaYonlendirilir()
        {
            //when
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOnlineOdemesiz);

            //then
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOnlineOdemesiz));
        }

        [Test]
        public void OnlineOdemeOldugunda_BankayaCekimIstegiGonderilir()
        {
            //when
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOnlineOdemeli);
            
            //then
            _bankaIletisimci.Verify(x=>x.CekimYap(_siparisBilgiOnlineOdemeli.KrediKartiBilgileri,_siparisBilgiOnlineOdemeli.ToplamTutar));
        }

        [Test]
        public void BankaCekimIstegiBasarisizOlursa_RestoranaSiparisVerilmez()
        {
            //given
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOnlineOdemeli.KrediKartiBilgileri, _siparisBilgiOnlineOdemeli.ToplamTutar)).Returns(false);

            //when
            SiparisSonuc siparisSonucu = _yemekSiparisMotoru.SiparisVer(_siparisBilgiOnlineOdemeli);

            //then
            Assert.IsFalse(siparisSonucu.SiparisBasariliMi);
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOnlineOdemeli),Times.Never);
        }

        [Test]
        public void BankaCekimIstegiBasariliOlursa_RestoranaSiparisVerilir()
        {
            //given
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOnlineOdemeli.KrediKartiBilgileri, _siparisBilgiOnlineOdemeli.ToplamTutar)).Returns(true);

            //when
            SiparisSonuc siparisSonucu = _yemekSiparisMotoru.SiparisVer(_siparisBilgiOnlineOdemeli);

            //then
            Assert.IsTrue(siparisSonucu.SiparisBasariliMi);
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOnlineOdemeli));
        }

        [Test]
        public void BankadanSonucDondugunde_VeritabaninaKayitEdilir()
        {
            //given 
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOnlineOdemeli.KrediKartiBilgileri, _siparisBilgiOnlineOdemeli.ToplamTutar)).Returns(true);
            
            //when
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOnlineOdemeli);

            //then
            _veritabaniIletisimci.Verify(x=>x.VeritabaninaKaydet(_siparisBilgiOnlineOdemeli));
        }

        [Test]
        public void RestoranSiparisiOnaylarsa_VeritabaninaKaydet()
        {
            //when
            _yemekSiparisMotoru.RestoranCevabiniIsle(_siparisBilgiOnlineOdemeli, true);

            //then
            _veritabaniIletisimci.Verify(x=>x.SiparisiOnaylandiOlarakKaydet(_siparisBilgiOnlineOdemeli));
        }

        [Test]
        public void RestoranSiparisiReddederse_VeritabaninaKaydet_RestoranaBilgiVer()
        {
            //when
            _yemekSiparisMotoru.RestoranCevabiniIsle(_siparisBilgiOnlineOdemeli, false);

            //then
            _veritabaniIletisimci.Verify(x=>x.SiparisiIptalOlarakKaydet(_siparisBilgiOnlineOdemeli));
            _cagriMerkeziIletisimci.Verify(x=>x.SiparisIptalBilgisiIlet(_siparisBilgiOnlineOdemeli));
        }

        [Test]
        public void CevapsizSiparisler5DakikaSonraIptalEdilir()
        {
            //given
            List<SiparisBilgileri> cevapsizSiparisler = new List<SiparisBilgileri>
            {
                new SiparisBilgileri { SiparisTarihi = DateTime.Now },
                new SiparisBilgileri { SiparisTarihi = DateTime.Now.AddMinutes(-5) },
                new SiparisBilgileri { SiparisTarihi = DateTime.Now },
                new SiparisBilgileri { SiparisTarihi = DateTime.Now.AddMinutes(-6) },
                new SiparisBilgileri { SiparisTarihi = DateTime.Now },
            };
            _veritabaniIletisimci.Setup(x => x.CevapsizSiparisleriAl()).Returns(cevapsizSiparisler);

            //when
            _yemekSiparisMotoru.BelliBirSuredirCevapAlinamayanSiparisleriIptalEt();

            //then
            _cagriMerkeziIletisimci.Verify(x=>x.SiparisIptalBilgisiIlet(It.IsAny<SiparisBilgileri>()),Times.Exactly(2));
            _veritabaniIletisimci.Verify(x=>x.SiparisiIptalOlarakKaydet(It.IsAny<SiparisBilgileri>()),Times.Exactly(2));
            _restoranIletisimci.Verify(x=>x.SiparisIptalIlet(It.IsAny<SiparisBilgileri>()),Times.Exactly(2));
        }


        [Test]
        public void Add()
        {
            //given(arrange)

            //when(act)

            //then(assert)
        }
    }
}
