using Business.AspNet;
using Business.Core;
using Business.Core.Annotations;
using Business.Core.Auth;
using Business.Core.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// <summary>
/// my Token
/// </summary>
[TokenCheck(message: "token illegal!")]//This is your token verification
[Logger(canWrite: false)]//Do not output log
public struct Token : IToken
{
    /// <summary>
    /// Key
    /// </summary>
    [JsonPropertyName("K")]
    public string Key { get; set; }

    /// <summary>
    /// Remote
    /// </summary>
    [JsonPropertyName("R")]
    public Remote Remote { get; set; }

    /// <summary>
    /// Path
    /// </summary>
    [JsonPropertyName("P")]
    public string Path { get; set; }

    /// <summary>
    /// Callback
    /// </summary>
    [JsonIgnore]
    public string Callback { get; set; }

    /// <summary>
    /// Origin
    /// </summary>
    [JsonIgnore]
    public Business.AspNet.Token.OriginValue Origin { get; set; }
}

//This is your token verification, must be inherited ArgumentAttribute
public class TokenCheck : ArgumentAttribute
{
    /* Injection
    
    [Injection]
    readonly HttpClient httpClient; //services.AddHttpClient();

    [Injection]
    readonly IMemoryCache cache; //services.AddMemoryCache();

    */

    //Good state specifications are important
    public TokenCheck(int state = -80, string message = null) : base(state, message) => Description = "{Alias} is not allowed to be empty";

    public override async ValueTask<IResult> Proces(dynamic value)
    {
        Token token = value;

        var key = token.Key;

        //..1: check token key
        if (string.IsNullOrWhiteSpace(key))
        {
            return ResultError(); //error
        }

        //..2: check token logic

        return ResultCreate(); //ok
    }
}

/// <summary>
/// BusinessBase
/// </summary>
[Common.JsonArg]
//[Common.NewtonsoftJsonArg]
public abstract class BusinessBase : Business.AspNet.BusinessBase
{
    //Override, using custom token In order to be able to process token data
    public sealed override ValueTask<IToken> GetToken(HttpContext context, Business.AspNet.Token token) => ValueTask.FromResult<IToken>(new Token
    {
        Origin = token.Origin,
        Key = token.Key,
        Remote = token.Remote,
        Callback = token.Callback,
        Path = token.Path
    });
}

#region WebSocket

/// <summary>
/// WebSocketManagement
/// </summary>
public class WebSocketManagement : Business.AspNet.WebSocketManagement
{
    /* Injection
    
    [Injection]
    readonly HttpClient httpClient; //services.AddHttpClient();

    [Injection]
    readonly IMemoryCache cache; //services.AddMemoryCache();

    */

    public sealed override async ValueTask<WebSocketAcceptReply> WebSocketAccept(HttpContext context)
    {
        // checked and return a token
        if (!context.Request.Query.TryGetValue("t", out Microsoft.Extensions.Primitives.StringValues token) || string.IsNullOrWhiteSpace(token))
        {
            return default;//prevent
        }

#if DEBUG
        $"WebSockets Add:{token} Connections:{WebSockets.Count + 1}".Log();
#endif
        return new WebSocketAcceptReply(token);
    }

    public override ValueTask WebSocketDispose(HttpContext context, string token)
    {
#if DEBUG
        $"WebSockets Remove:{token} Connectionss:{WebSockets.Count}".Log();
#endif
        return default;
    }

}

#endregion
