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
        private SiparisBilgileri _siparisBilgiOdemeli;
        private SiparisBilgileri _siparisBilgiOdemesiz;

        [SetUp]
        public void Init()
        {
            _restoranIletisimci = new Mock<IRestoranIletisimci>();
            _bankaIletisimci = new Mock<IBankaIletisimci>();
            _veritabaniIletisimci = new Mock<IVeritabaniIletisimci>();
            _cagriMerkeziIletisimci = new Mock<ICagriMerkeziIletisimci>();
            _yemekSiparisMotoru = new YemekSiparisMotoru(_restoranIletisimci.Object,_bankaIletisimci.Object,_veritabaniIletisimci.Object,_cagriMerkeziIletisimci.Object);
            
            _siparisBilgiOdemeli = new SiparisBilgileri
            {
                KrediKartiBilgileri = new KrediKartiBilgileri(),
                OdemeTipi = SiparisOdemeTip.OnlineKrediKarti,
                SiparisTarihi = DateTime.Now,
                ToplamTutar = 100
            };

            _siparisBilgiOdemeli = new SiparisBilgileri
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
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOdemesiz);

            //then
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOdemesiz));
        }

        [Test]
        public void OnlineOdemeOldugunda_BankayaCekimIstegiGonderilir()
        {
            //when
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOdemeli);
            
            //then
            _bankaIletisimci.Verify(x=>x.CekimYap(_siparisBilgiOdemeli.KrediKartiBilgileri,_siparisBilgiOdemeli.ToplamTutar));
        }

        [Test]
        public void BankaCekimIstegiBasarisizOlursa_RestoranaSiparisVerilmez()
        {
            //given
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOdemeli.KrediKartiBilgileri, _siparisBilgiOdemeli.ToplamTutar)).Returns(false);

            //when
            SiparisSonuc siparisSonucu = _yemekSiparisMotoru.SiparisVer(_siparisBilgiOdemeli);

            //then
            Assert.IsFalse(siparisSonucu.SiparisBasariliMi);
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOdemeli),Times.Never);
        }

        [Test]
        public void BankaCekimIstegiBasariliOlursa_RestoranaSiparisVerilir()
        {
            //given
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOdemeli.KrediKartiBilgileri, _siparisBilgiOdemeli.ToplamTutar)).Returns(true);

            //when
            SiparisSonuc siparisSonucu = _yemekSiparisMotoru.SiparisVer(_siparisBilgiOdemeli);

            //then
            Assert.IsTrue(siparisSonucu.SiparisBasariliMi);
            _restoranIletisimci.Verify(x=>x.SiparisBilgileriniGonder(_siparisBilgiOdemeli));
        }

        [Test]
        public void BankadanSonucDondugunde_VeritabaninaKayitEdilir()
        {
            //given 
            _bankaIletisimci.Setup(x => x.CekimYap(_siparisBilgiOdemeli.KrediKartiBilgileri, _siparisBilgiOdemeli.ToplamTutar)).Returns(true);
            
            //when
            _yemekSiparisMotoru.SiparisVer(_siparisBilgiOdemeli);

            //then
            _veritabaniIletisimci.Verify(x=>x.VeritabaninaKaydet(_siparisBilgiOdemeli));
        }

        [Test]
        public void RestoranSiparisiOnaylarsa_VeritabaninaKaydet()
        {
            //when
            _yemekSiparisMotoru.RestoranCevabiniIsle(_siparisBilgiOdemeli, true);

            //then
            _veritabaniIletisimci.Verify(x=>x.SiparisiOnaylandiOlarakKaydet(_siparisBilgiOdemeli));
        }

        [Test]
        public void RestoranSiparisiReddederse_VeritabaninaKaydet_RestoranaBilgiVer()
        {
            //when
            _yemekSiparisMotoru.RestoranCevabiniIsle(_siparisBilgiOdemeli, false);

            //then
            _veritabaniIletisimci.Verify(x=>x.SiparisiIptalOlarakKaydet(_siparisBilgiOdemeli));
            _cagriMerkeziIletisimci.Verify(x=>x.SiparisIptalBilgisiIlet(_siparisBilgiOdemeli));
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
