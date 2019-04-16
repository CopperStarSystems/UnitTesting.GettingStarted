//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.CreditDecisionService.cs
// 2017/01/13
//  --------------------------------------------------------------------------------------

using System.Threading;

namespace UnitTesting.GettingStarted.CreditDecisionExample
{
    public class CreditDecisionService : ICreditDecisionService
    {
        readonly IExternalCreditDecisionService externalService;

        public CreditDecisionService(IExternalCreditDecisionService externalService)
        {
            this.externalService = externalService;
        }

        public string GetDecision(int creditScore)
        {
            var adjustedScore = externalService.InvokeExternalService(creditScore);
            if (adjustedScore < 550)
                return "Declined";
            if (adjustedScore <= 675)
                return "Maybe";
            return "We look forward to doing business with you!";
        }
    }
}