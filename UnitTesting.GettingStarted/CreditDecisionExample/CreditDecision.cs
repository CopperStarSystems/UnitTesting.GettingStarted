//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.CreditDecision.cs
// 2017/01/13
//  --------------------------------------------------------------------------------------

namespace UnitTesting.GettingStarted.CreditDecisionExample
{
    public class CreditDecision
    {
        readonly ICreditDecisionService creditDecisionService;

        public CreditDecision(ICreditDecisionService creditDecisionService)
        {
            this.creditDecisionService = creditDecisionService;
        }

        public string MakeCreditDecision(int creditScore)
        {
            return creditDecisionService.GetDecision(creditScore);
        }
    }
}