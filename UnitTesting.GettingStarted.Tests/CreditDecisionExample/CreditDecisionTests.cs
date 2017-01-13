//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.Tests.CreditDecisionTests.cs
// 2017/01/13
//  --------------------------------------------------------------------------------------

using Moq;
using NUnit.Framework;
using UnitTesting.GettingStarted.CreditDecisionExample;

namespace UnitTesting.GettingStarted.Tests.CreditDecisionExample
{
    [TestFixture]
    public class CreditDecisionTests
    {
        Mock<ICreditDecisionService> mockCreditDecisionService;

        CreditDecision systemUnderTest;

        [TestCase(100, "Declined")]
        [TestCase(549, "Declined")]
        [TestCase(550, "Maybe")]
        [TestCase(675, "Maybe")]
        [TestCase(676, "We look forward to doing business with you!")]
        public void MakeCreditDecision_Always_ReturnsExpectedResult(int creditScore, string expectedResult)
        {
            mockCreditDecisionService = new Mock<ICreditDecisionService>(MockBehavior.Strict);
            mockCreditDecisionService.Setup(p => p.GetDecision(creditScore)).Returns(expectedResult);

            systemUnderTest = new CreditDecision(mockCreditDecisionService.Object);
            var result = systemUnderTest.MakeCreditDecision(creditScore);

            Assert.That(result, Is.EqualTo(expectedResult));

            mockCreditDecisionService.VerifyAll();
        }
    }
}