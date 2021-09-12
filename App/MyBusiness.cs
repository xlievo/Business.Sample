using Business.Core;
using Business.Core.Annotations;
using Business.Core.Auth;
using Business.Core.Result;
using Business.AspNet;
using LinqToDB;
using DataModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Hosting;

//Maybe you need a custom base class? To unify the processing of logs and token
public class MyBusiness : BusinessBase
{
    #region Injection services.Add... or app.CreateBusiness().UseInjection()

    [Injection]
    readonly HttpClient httpClient;

    [Injection]
    readonly IHostApplicationLifetime appLifetime;
    /*
    //Constructor injection
    public MyBusiness(HttpClient httpClient, IHostApplicationLifetime appLifetime)
    {
        //appLifetime.StopApplication(); 
        //Utils.Hosting.AppLifetime You can also manipulate the lifecycle using static objects

        this.httpClient = httpClient;
    }
    */
    #endregion

    //My first business logic
    //Logical method must be public virtual!
    //If inherited Business.AspNet.BusinessBase Base class, you just need to concentrate on writing logical methods!
    public virtual async ValueTask<IResult<MyLogicArg>> MyLogic(Token token, Context context, HttpFile files, MyLogicArg arg)
    {
        return this.ResultCreate(arg);
    }

    //[Parameters] must correspond to class
    public virtual async ValueTask<IResult<MyParametersArg>> MyParameters(Token token, [Parameters] MyParametersArg arg)
    {
        return this.ResultCreate(arg);
    }

    #region WebSocket push case

    [Push]
    public virtual async ValueTask<IResult<WebSocketPushLogicData>> WebSocketPushLogic(Token token, MyLogicArg arg, [Ignore(IgnoreMode.Arg)] params string[] id)
    {
        var pushData = new WebSocketPushLogicData { C = arg.A, D = arg.B };

        this.SendAsync(pushData, id);// The push data must be consistent with the return object

        return this.ResultCreate(pushData);// Used as a return standard convention
    }

    #endregion
}