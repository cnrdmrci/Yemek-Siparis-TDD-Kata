# Unit Testing  (Nunit)

### Unit testing avantajları
- Geliştirme yaparken karşılaşılabilecek hataların erken tespiti.
- Güvenilir (Reliable) kod yazmak.
- Sürdürülebilir (Maintainable) kod yazmak.
- Hızlı bir şekilde test edebilmek.

### Nunit some of Attributes

- TestFixture
> ilgili sınıfın Nunit test yöntemleri içerdiğini belirtir.

- Setup
> Test koleksiyonu içerisindeki her bir tset öncesinde çağırılır.

- TearDown
> Test koleksiyonu içerisindeki her bir test çalıştıktan sonra çağırılacak fonksiyondur.

- Test
> Test edileceğini belirtir.

- Category
> Test fonksiyonlarını kategorilendirmeye yarar. Toplu şekilde bu gruplar çalıştırılabilir.

- Description
> Açıklama ekler.

- Ignore
> Testin çalışırılması devredışı bırakılabilir.

- Repeat
> Test fonksiyonunun kaç kez çalıştırılacağını belirtir.

- MaxTime
> Milisaniye cinsinden belirlenen sürede tamamlanmasını kontrol eder.

- Order
> Test çalıştırma sırasını belirtir.

- Combinatorial
```csharp
[Test, Combinatorial]
public void MyTest([Values(1,2,3)] int x,[Values("A","B")] string s)
{ /* ... */ };

/*
	MyTest(1, "A")
	MyTest(1, "B")
	MyTest(2, "A")
	MyTest(2, "B")
	MyTest(3, "A")
	MyTest(3, "B")
*/
```
- Sequential

```csharp
[Test, Sequential]
public void MyTest([Values(1,2,3)] int x, [Values("A","B")] string s)
{ /* ... */ };

/*
	MyTest(1, "A")
	MyTest(2, "B")
	MyTest(3, null)
*/
```
- TestCase
> Test fonksiyonunun parametre ile çalıştırılmasını sağlar.

```csharp
[TestCase(12,3,4)]
[TestCase(12,2,6)]
[TestCase(12,4,3)]
public void DivideTest(int n, int d, int q)
{
  Assert.AreEqual( q, n / d );
};

[TestCase(12,3, Result=4)]
[TestCase(12,2, Result=6)]
[TestCase(12,4, Result=3)]
public int DivideTest(int n, int d)
{
  return( n / d );
};
```

- TestCaseSource
> Test fonksiyonunun belirli bir değerden alınmasını sağlar.
```csharp
object[] DivideCases =
{
    new object[] { 12, 3, 4 },
    new object[] { 12, 2, 6 },
    new object[] { 12, 4, 3 }
};

int[] EvenNumbers = new int[] { 2, 4, 6, 8 };

[Test, TestCaseSource("DivideCases")]
public void DivideTest(int n, int d, int q)
{
    Assert.AreEqual( q, n / d );
}

[Test, TestCaseSource("EvenNumbers")]
public void TestMethod(int num)
{
    Assert.IsTrue( num % 2 == 0 );
};
```

### Örnek temel kullanım
```csharp
namespace NUnit.Tests
{
  using System;
  using NUnit.Framework;

  [TestFixture]
  public class SuccessTests
  {
    [SetUp] public void Init()
    { /* ... */ }

    [TearDown] public void Cleanup()
    { /* ... */ }

    [Test] public void Add()
    { 
    	//given(arrange)

    	//when(act)

    	//then(assert)
    }
  }
}
```
