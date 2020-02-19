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

### Some of Assertions
```csharp

//Equality Asserts
Assert.AreEqual( int expected, int actual );
Assert.AreEqual( 5, 5.0 ); //succeeds

//Condition Asserts
Assert.IsTrue( bool condition );
Assert.True( bool condition );
Assert.IsFalse( bool condition);
Assert.False( bool condition);
Assert.IsNull( object anObject );
Assert.Null( object anObject );
Assert.IsNotNull( object anObject );
Assert.NotNull( object anObject );
Assert.IsEmpty( string aString ); //""
Assert.IsNotEmpty( string aString );
Assert.IsEmpty( ICollection collection );
Assert.IsNotEmpty( ICollection collection );

//Comparison Asserts ( Assert.Greater( x, y ) asserts that x is greater than y ( x > y ). )
Assert.Greater( int arg1, int arg2 ); //(arg1>arg2)
Assert.GreaterOrEqual( int arg1, int arg2 ); //(arg1>=arg2)
Assert.Less( int arg1, int arg2 ); //(arg1<arg2)
Assert.LessOrEqual( int arg1, int arg2 ); //(arg1<=arg2)

//Type Asserts
Assert.IsInstanceOf<T>( object actual ); //Assert.IsInstanceOf<double>(1.9); then pass
Assert.IsNotInstanceOf<T>( object actual ); //Assert.IsNotInstanceOf(typeof(int), 1.9);

//Exception Asserts
Assert.Throws( Type expectedExceptionType, TestDelegate code );
Assert.Throws<T>( TestDelegate code );
Assert.Throws( typeof(ArgumentException),delegate { throw new ArgumentException(); } );
Assert.Throws<ArgumentException>(delegate { throw new ArgumentException(); } );

//Utiliy Asserts
Assert.Pass();
Assert.Fail();
Assert.Ignore();//ünlem
Assert.Fail( string message );

//String Assers
StringAssert.Contains( string expected, string actual ); //StringAssert.Contains("ela", "selam"); then pass
StringAssert.StartsWith( string expected, string actual ); //StringAssert.StartsWith("sel", "selam");
StringAssert.EndsWith( string expected, string actual ); //StringAssert.EndsWith("m","selam");
StringAssert.IsMatch( string regexPattern, string actual ); //StringAssert.IsMatch("a?bc", "12a3bc45"); //Regex!


//-------Assert.That()--------;

//-Equal Constraint Example
Assert.That(5,Is.EqualTo(5));
Assert.That(result, Is.Not.EqualTo(7));
Assert.That(result, Is.EqualTo("abcdefg"));
Assert.That(result, Is.EqualTo("ABCDEFG").IgnoreCase);

//-Greater Than Constraint Example
Assert.That(result, Is.GreaterThan(2));
Assert.That(result, Is.GreaterThanOrEqualTo(5));

//-Less Than Constraint Example
Assert.That(result, Is.LessThan(9));
Assert.That(result, Is.LessThanOrEqualTo(5));

//-Ranges Example
Assert.That(result, Is.InRange(5, 10));

//-Substring Constraint Example
Assert.That(result, Does.Contain("def").IgnoreCase);
Assert.That(result, Does.Not.Contain("igk").IgnoreCase);

//-Empty Example
Assert.That(result, Is.Empty);
Assert.That(result, Is.Not.Empty);

//-Starts With / Ends With Examples
Assert.That(result, Does.StartWith("abc"));
Assert.That(result, Does.Not.StartWith("efg"));
 
Assert.That(result, Does.EndWith("efg"));
Assert.That(result, Does.Not.EndWith("mno"));

//-Regex Constraint Example
string result = "abcdefg";
Assert.That(result, Does.Match("a*g"));
Assert.That(result, Does.Not.Match("m*n"));


//----Collection Constraints----

int[] array = new int[] { 1, 2, 3, 4, 5 };
//-Not Null Example
Assert.That(array, Is.All.Not.Null);

Assert.That(array, Is.All.GreaterThan(0));
Assert.That(array, Is.All.LessThan(10));
Assert.That(array, Is.All.InstanceOf<Int32>());
Assert.That(array, Is.Empty);
Assert.That(array, Is.Not.Empty);
Assert.That(array, Has.Exactly(5).Items);
Assert.That(array, Contains.Item(4));
Assert.That(array, Is.Ordered.Ascending);
Assert.That(array, Is.Ordered.Descending);

List<Employee> employees = new List<Employee>();
employees.Add(new Employee { Age = 32 });
employees.Add(new Employee { Age = 49 });
employees.Add(new Employee { Age = 57 });
 
Assert.That(employees, Is.Ordered.Ascending.By("Age"));
Assert.That(employees, Is.Ordered.Descending.By("Age"));

Assert.That(employees, Is.Ordered.Ascending.By("Age").Then.Descending.By("Name"));

//-SuperSet / SubSet Examples
int[] array = new int[] { 1, 2, 3, 4, 5 };
int[] array2 = { 3, 4 };
Assert.That(array2, Is.SubsetOf(array));


Assert.That(array, Is.Null);
Assert.That(array, Is.Not.Null);

//-
bool result = array.Length > 0;
Assert.That(result, Is.True);
 
Assert.That(result, Is.False);


//-
Assert.That(array, Is.Empty);

//-AND,OR,NOT constraint example
Assert.That(result, Is.GreaterThan(4).And.LessThan(10));
Assert.That(result, Is.LessThan(1).Or.GreaterThan(4));
Assert.That(result, Is.Not.EqualTo(7));
```

# Mock 

### Mock nedir?
> Bir objenin yerine geçen fake bir objeyi istediğimiz gibi davranış kazandırabileceğimiz işlemdir.

### Mock objesi oluşturma
> var mockT = new Mock<T>();
> var mockT2 = new Mock<T>(MockBehavior.Strict);

### Method Mock
- Method mock olarak bilinmesi gereken 3 adet davranış bulunmakta;
	1. MockBehavior.Strict
	> Mock’lu method çağrıldığında setup edilmemiş ise exception fırlatır.

	2. MockBehavior.Loose
	> Hiç bir zaman exception fırlatmaz. Çağrı sonrası exception yerine return tipi ne ise default halini döner, yeni object oluşturmaz. Dönüş türünüz int ise default value “0” olduğu için 0 döner. Eğer bir class ise null döner.

	3. MockBehavior.Default

### Methods
```csharp
var mock = new Mock<IFoo>();
mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

// access invocation arguments when returning a value
mock.Setup(x => x.DoSomethingStringy(It.IsAny<string>())).Returns((string s) => s.ToLower());
// Multiple parameters overloads available

// throwing when invoked with specific parameters
mock.Setup(foo => foo.DoSomething("reset")).Throws<InvalidOperationException>();
mock.Setup(foo => foo.DoSomething("")).Throws(new ArgumentException("command"));

// lazy evaluating return value
var count = 1;
mock.Setup(foo => foo.GetCount()).Returns(() => count);

```

### Matching Arguments
```csharp
// any value
mock.Setup(foo => foo.DoSomething(It.IsAny<string>())).Returns(true);

// matching Func<int>, lazy evaluated
mock.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 0))).Returns(true); 

// matching ranges
mock.Setup(foo => foo.Add(It.IsInRange<int>(0, 10, Range.Inclusive))).Returns(true); 

// matching regex
mock.Setup(x => x.DoSomethingStringy(It.IsRegex("[a-d]+", RegexOptions.IgnoreCase))).Returns("foo");
```

### Properties
```csharp
mock.Setup(foo => foo.Name).Returns("bar");

// auto-mocking hierarchies (a.k.a. recursive mocks)
mock.Setup(foo => foo.Bar.Baz.Name).Returns("baz");

// expects an invocation to set the value to "foo"
mock.SetupSet(foo => foo.Name = "foo");

// or verify the setter directly
mock.VerifySet(foo => foo.Name = "foo");

//Setup a property so that it will automatically start tracking its value (also known as Stub):

// start "tracking" sets/gets to this property
mock.SetupProperty(f => f.Name);

// alternatively, provide a default value for the stubbed property
mock.SetupProperty(f => f.Name, "foo");

//Stub all properties on a mock
mock.SetupAllProperties();
```

### Miscellaneous
```csharp
//Setting up a member to return different values / throw exceptions on sequential calls:
var mock = new Mock<IFoo>();
mock.SetupSequence(f => f.GetCount())
    .Returns(3)  // will be returned on 1st invocation
    .Returns(2)  // will be returned on 2nd invocation
    .Returns(1)  // will be returned on 3rd invocation
    .Returns(0)  // will be returned on 4th invocation
    .Throws(new InvalidOperationException());  // will be thrown on 5th invocation

//Setting expectations for protected members (you can't get IntelliSense for these, so you access them using the member name as a string):
// in the test
var mock = new Mock<CommandBase>();
mock.Protected()
     .Setup<int>("Execute")
     .Returns(5);

// if you need argument matching, you MUST use ItExpr rather than It
// planning on improving this for vNext (see below for an alternative in Moq 4.8)
mock.Protected()
    .Setup<bool>("Execute",
        ItExpr.IsAny<string>())
    .Returns(true);
```

### Verification
```csharp
mock.Verify(foo => foo.DoSomething("ping"));

// Verify with custom error message for failure
mock.Verify(foo => foo.DoSomething("ping"), "When doing operation X, the service should be pinged always");

// Method should never be called
mock.Verify(foo => foo.DoSomething("ping"), Times.Never());

// Called at least once
mock.Verify(foo => foo.DoSomething("ping"), Times.AtLeastOnce());

//Times parametreleri;
//Never, Once, AtLeast, AtLeastOnce, AtMost, AtMostOnce, Between, Exactly
//En fazla 1 kere çağırması için AtMostOnce ya da Exactly(1) gibi kontrollerle yapılabilir.

// Verify getter invocation, regardless of value.
mock.VerifyGet(foo => foo.Name);

// Verify setter invocation, regardless of value.
mock.VerifySet(foo => foo.Name);

// Verify setter called with specific value
mock.VerifySet(foo => foo.Name ="foo");

// Verify setter with an argument matcher
mock.VerifySet(foo => foo.Value = It.IsInRange(1, 5, Range.Inclusive));

// Verify that no other invocations were made other than those already verified (requires Moq 4.8 or later)
mock.VerifyNoOtherCalls();
```

### Callbacks
```csharp
var mock = new Mock<IFoo>();
var calls = 0;
var callArgs = new List<string>();

mock.Setup(foo => foo.DoSomething("ping"))
    .Callback(() => calls++)
    .Returns(true);

// access invocation arguments
mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
    .Callback((string s) => callArgs.Add(s))
    .Returns(true);

// alternate equivalent generic method syntax
mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
    .Callback<string>(s => callArgs.Add(s))
    .Returns(true);

// access arguments for methods with multiple parameters
mock.Setup(foo => foo.DoSomething(It.IsAny<int>(), It.IsAny<string>()))
    .Callback<int, string>((i, s) => callArgs.Add(s))
    .Returns(true);

// callbacks can be specified before and after invocation
mock.Setup(foo => foo.DoSomething("ping"))
    .Callback(() => Console.WriteLine("Before returns"))
    .Returns(true)
    .Callback(() => Console.WriteLine("After returns"));

```

### Matching Generic Type Arguments (Moq 4.13+)
```csharp
public interface IFoo
{
    bool M1<T>();
    bool M2<T>(T arg);
}

var mock = new Mock<IFoo>();

// matches any type argument:
mock.Setup(m => m.M1<It.IsAnyType>()).Returns(true);

// matches only type arguments that are subtypes of / implement T:
mock.Setup(m => m.M1<It.IsSubtype<T>>()).Returns(true);

// use of type matchers is allowed in the argument list:
mock.Setup(m => m.M2(It.IsAny<It.IsAnyType>())).Returns(true);
mock.Setup(m => m.M2(It.IsAny<It.IsSubtype<T>>())).Returns(true);
```