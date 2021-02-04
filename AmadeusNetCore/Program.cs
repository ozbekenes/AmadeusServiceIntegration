using AmadeusNetCore.Handler;
using System;
using System.Threading.Tasks;
namespace AmadeusNetCore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var serviceHandler = new ServiceHandler("1ASIWHARHRT", new Dispatcher.LoggingBehavior(new SecurityTokenInspector("userName", "password")));
            try
            {
                await serviceHandler.Air_FlightInfo(SessionHandler.TransactionStatusCode.Start, TransactionFlowLinkHandler.TransactionFlowLinkAction.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            // Stateless call
        }
    }
}
