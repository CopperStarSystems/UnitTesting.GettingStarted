//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.Tests.CreditDecisionServiceTests.cs
// 2019/04/16
//  --------------------------------------------------------------------------------------

using Moq;
using NUnit.Framework;
using UnitTesting.GettingStarted.CreditDecisionExample;

namespace UnitTesting.GettingStarted.Tests.CreditDecisionExample
{
    [TestFixture]
    public class CreditDecisionServiceTests
    {
        Mock<IExternalCreditDecisionService> mockExternalCreditDecisionService;
        CreditDecisionService systemUnderTest;

        public void GetDecision_Always_PerformsExpectedWork(int creditScore, string expectedMessage)
        {
            mockExternalCreditDecisionService.Setup(p => p.InvokeExternalService(It.IsAny<int>()))
                                             .Returns(1);
            systemUnderTest = new CreditDecisionService(mockExternalCreditDecisionService.Object);

            // Just a random-ish number, we don't need test cases here because we're literally just verifying that
            // our SystemUnderTest asks the external service for an answer.
            //
            // For the same reason, we discard the returned result, we'll verify that in the other test.
            systemUnderTest.GetDecision(150);
            mockExternalCreditDecisionService.VerifyAll();
        }

        [TestCase(100, "Declined")]
        [TestCase(549, "Declined")]
        [TestCase(550, "Maybe")]
        [TestCase(675, "Maybe")]
        [TestCase(676, "We look forward to doing business with you!")]
        public void GetDecision_Always_ReturnsExpectedMessage(int creditScore, string expectedMessage)
        {
            mockExternalCreditDecisionService.Setup(p => p.InvokeExternalService(creditScore))
                                             .Returns(creditScore);
            systemUnderTest = new CreditDecisionService(mockExternalCreditDecisionService.Object);

            var result = systemUnderTest.GetDecision(creditScore);
            Assert.That(result, Is.EqualTo(expectedMessage));
        }

        [SetUp]
        public void SetUp()
        {
            mockExternalCreditDecisionService = new Mock<IExternalCreditDecisionService>(MockBehavior.Strict);
        }
    }
}