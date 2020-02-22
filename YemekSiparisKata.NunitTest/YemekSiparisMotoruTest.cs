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
        private SiparisBilgi _siparisBilgiOdemeli;
        private SiparisBilgi _siparisBilgiOdemesiz;

        [SetUp]
        public void Init()
        {
            _restoranIletisimci = new Mock<IRestoranIletisimci>();
            _bankaIletisimci = new Mock<IBankaIletisimci>();
            _veritabaniIletisimci = new Mock<IVeritabaniIletisimci>();
            _cagriMerkeziIletisimci = new Mock<ICagriMerkeziIletisimci>();
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
