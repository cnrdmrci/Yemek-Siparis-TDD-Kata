using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace YemekSiparisKata.NunitTest
{
    [TestFixture]
    public class YemekSiparisMotoruTest
    {
        private IRestoranIletisimci _restoranIletisimci;
        private IBankaIletisimci _bankaIletisimci;
        private IVeritabaniIletisimci _veritabaniIletisimci;
        private ICagriMerkeziIletisimci _cagriMerkeziIletisimci;
        private YemekSiparisMotoru _yemekSiparisMotoru;
        private SiparisBilgi _siparisBilgiOdemeli;
        private SiparisBilgi _siparisBilgiOdemesiz;

        [SetUp]
        public void Init()
        {
            
        }

        [TearDown]
        public void Cleanup()
        { /* ... */ }

        [Test]
        public void Add()
        {
            //given(arrange)

            //when(act)

            //then(assert)
        }
    }
}
