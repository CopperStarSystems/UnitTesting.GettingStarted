//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.Tests.BadCreditDecisionTests.cs
// 2017/01/13
//  --------------------------------------------------------------------------------------

using NUnit.Framework;
using UnitTesting.GettingStarted.CreditDecisionExample;

namespace UnitTesting.GettingStarted.Tests.CreditDecisionExample
{
    /************************************************************
     * Note:  This test fixture is intentionally disabled by 
     * default because it illustrates a poor development practice.
     *   
     * To enable these tests, remove the Ignore(...)
     * attribute below.
     ***********************************************************/

    /************************************************************
    * Important Note:  This is a "You're Doing It Wrong" example.
    * 
    * Unit tests should never use concrete external dependencies
    * because:
    *   - They may have side effects (i.e. inserting records into 
    *   database)
    *   - They may be slow (due to compute time or network
    *   latency)
    *   - If you're using actual concrete dependencies, you are 
    *   not testing your component in isolation - effectively
    *   this turns your unit test into an integration test
    *   
    * For a better implementation, look at CreditDecisionTests.cs
    ************************************************************/

    [TestFixture]
    //[Ignore("Remove this IgnoreAttribute to execute these tests.")]
    public class BadCreditDecisionTests
    {
        readonly CreditDecisionService creditDecisionService = new CreditDecisionService();

        CreditDecision systemUnderTest;

        [TestCase(100, "Declined")]
        [TestCase(549, "Declined")]
        [TestCase(550, "Maybe")]
        [TestCase(675, "Maybe")]
        [TestCase(676, "We look forward to doing business with you!")]
        public void MakeCreditDecision_Always_ReturnsExpectedResult(int creditScore, string expectedResult)
        {
            // We're using a concrete CreditDecision class instead of a mock.
            // If we look at CreditDecision.cs, each call to GetCreditDecision
            // has a built-in delay of 2.5 seconds before it returns a value.
            //
            // Since we have 5 test cases above, running this test will take a total
            // of 12.5 seconds to execute, as compared to the same test in
            // CreditDecisionTests, which executes in a matter of milliseconds.
            //
            // Although it may not seem like much now, as your test suite grows, 
            // the time sink rapidly becomes painful.
            systemUnderTest = new CreditDecision(creditDecisionService);

            // Each time we execute this method, it will take 2.5 seconds before we
            // get a result
            var result = systemUnderTest.MakeCreditDecision(creditScore);

            // Fortunately assertion works as normal...
            Assert.That(result, Is.EqualTo(expectedResult));

            // Ah, but we can no longer 'prove' (using the test) that our 
            // MakeCreditDecision method:
            //   - Actually called into CreditDecisionService
            //   - Passed the correct parameters to CreditDecisionService
            //   - Returned the actual value from CreditDecisionService without
            //   modifying it
            // In effect, all we know is that this method (hopefully) 
            // returned the correct result, but we have no way to tell how 
            // it actually arrived at that result.
            //
            // creditDecisionService.VerifyAll();
        }
    }
}