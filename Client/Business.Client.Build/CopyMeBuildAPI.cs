using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Client;

public class Host_localhost_5000
{
    public class MyBusiness : API<MyBusiness>
    {
        public MyBusiness(Protocol protocol) : base(protocol, "MyBusiness") { }

        #region Call

        public async ValueTask<IResult<MyLogic_Result>> MyLogic(MyLogic_Arg arg, string token = null)
        {
            return await Config.GetResult<ResultObject<MyLogic_Result>, MyLogic_Arg>(this, arg, token, true, true);
        }

        public async ValueTask<IResult<MyParameters_Result>> MyParameters(MyParameters_Arg arg, string token = null)
        {
            return await Config.GetResult<ResultObject<MyParameters_Result>, MyParameters_Arg>(this, arg, token, true, true);
        }

        #endregion

        #region Receive

        public static Action<WebSocketPushLogic_Result> WebSocketPushLogic { get; set; }

        #endregion

        #region DTO

        /// <summary>
        /// MyLogicArg!
        /// </summary>
        public class MyLogic_Arg
        {
            /// <summary>
            /// AAA
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// BBB
            /// </summary>
            public string B { get; set; }
        }

        /// <summary>
        /// MyLogicArg!
        /// </summary>
        public class MyLogic_Result
        {
            /// <summary>
            /// AAA
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// BBB
            /// </summary>
            public string B { get; set; }
        }

        /// <summary>
        /// MyLogicArg!
        /// </summary>
        public class MyParameters_Arg
        {
            /// <summary>
            /// AAA
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// BBB
            /// </summary>
            public string B { get; set; }
        }

        /// <summary>
        /// MyLogicArg!
        /// </summary>
        public class MyParameters_Result
        {
            /// <summary>
            /// AAA
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// BBB
            /// </summary>
            public string B { get; set; }
        }

        /// <summary>
        /// API data
        /// </summary>
        public class WebSocketPushLogic_Arg
        {
            /// <summary>
            /// MyLogicArg!
            /// </summary>
            public MyLogicArg arg { get; set; }

            public string aaa { get; set; }

            /// <summary>
            /// MyLogicArg!
            /// </summary>
            public class MyLogicArg
            {
                /// <summary>
                /// AAA
                /// </summary>
                public string A { get; set; }

                /// <summary>
                /// BBB
                /// </summary>
                public string B { get; set; }
            }
        }

        public class WebSocketPushLogic_Result
        {
            public WebSocketPushLogicResult Item1 { get; set; }

            public string Item2 { get; set; }

            public class WebSocketPushLogicResult
            {
                /// <summary>
                /// CCC
                /// </summary>
                public string C { get; set; }

                /// <summary>
                /// DDD
                /// </summary>
                public string D { get; set; }
            }
        }

        #endregion
    }

    public static Config Config { get; } = new Config("http://localhost:5000", new Utils.RouteCTD("c", "t", "d"), MessagePack.MessagePackCompression.Lz4Block);

    static Host_localhost_5000()
    {
        Utils.Pushs.Add(new BusinessInfo("MyBusiness", "WebSocketPushLogic"), new Push(typeof(MyBusiness.WebSocketPushLogic_Result), c => MyBusiness.WebSocketPushLogic?.Invoke(c as MyBusiness.WebSocketPushLogic_Result)));
    }
}