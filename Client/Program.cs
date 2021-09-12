using System;
using System.Threading.Tasks;
using Business.Client;
using static Host_localhost_5000;

namespace Client
{
    class Program
    {
        const string token = "123456";// login

        static async Task Main(string[] args)
        {
            Console.WriteLine("C# Client Sample!");

            #region WebSocket set

            MyBusiness.WebSocketPushLogic = c =>
            {
                Console.WriteLine($"Receive [WebSocketPushLogic] {c.JsonSerialize()}");
            };

            // connect
            var open = await Host_localhost_5000.Config.UseWebSocket(token);

            if (!open)
            {
                Console.WriteLine($"connect {Host_localhost_5000.Config.options.Host} fail");
            }

            #endregion

            var result = await MyBusiness.WebSocket.MyLogic(new MyBusiness.MyLogic_Arg { A = "111", B = "222" });

            Console.WriteLine($"WebSocketCall [MyLogic] {result.Data.JsonSerialize()}");

            var result2 = await MyBusiness.Http.MyLogic(new MyBusiness.MyLogic_Arg { A = "111", B = "222" }, token);

            Console.WriteLine($"HttpCall [MyLogic] {result2.Data.JsonSerialize()}");

            Console.Read();
        }
    }
}
