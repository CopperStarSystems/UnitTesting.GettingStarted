//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.ICreditDecisionService.cs
// 2017/01/13
//  --------------------------------------------------------------------------------------

namespace UnitTesting.GettingStarted.CreditDecisionExample
{
    public interface ICreditDecisionService
    {
        string GetDecision(int creditScore);
    }
}