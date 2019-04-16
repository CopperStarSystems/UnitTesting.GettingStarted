//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.IExternalCreditDecisionService.cs
// 2019/04/16
//  --------------------------------------------------------------------------------------

namespace UnitTesting.GettingStarted.CreditDecisionExample
{
    public interface IExternalCreditDecisionService
    {
        int InvokeExternalService(int creditScore);
    }
}