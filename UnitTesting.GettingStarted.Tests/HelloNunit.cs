//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.Tests.HelloNunit.cs
// 2017/01/12
//  --------------------------------------------------------------------------------------

using NUnit.Framework;

namespace UnitTesting.GettingStarted.Tests
{
    [TestFixture]
    public class HelloNunit
    {
        [Test]
        public void TestHelloNunit()
        {
            Assert.That(true, Is.False);
        }
    }
}