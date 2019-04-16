//  --------------------------------------------------------------------------------------
// UnitTesting.GettingStarted.ExternalCreditDecisionService.cs
// 2019/04/16
//  --------------------------------------------------------------------------------------

using System.Threading;

namespace UnitTesting.GettingStarted.CreditDecisionExample
{
    public class ExternalCreditDecisionService : IExternalCreditDecisionService
    {
        public int InvokeExternalService(int creditScore)
        {
            // Moved the thread.sleep from CreditDecisionService since the external service
            // is the source of latency.
            Thread.Sleep(2500);
            
            // This is just a simple example, so just return whatever was passed in
            return creditScore;
        }
    }
}