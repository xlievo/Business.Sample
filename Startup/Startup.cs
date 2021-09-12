using Business.AspNet;
using Business.Core;
using Business.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Net.Http;

//namespace $ext_safeprojectname$
//{

//}

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        //Configure cross domain policy
        services.AddCors(options =>
        {
            options.AddPolicy("any", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        services.AddControllers(option => option.EnableEndpointRouting = false)
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);
            //.AddNewtonsoftJson();

        services.AddHttpClient(string.Empty)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseDefaultCredentials = true,
            });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
    {
        app.UseCors("any");//API static documents need cross domain support

        //Using third party log components
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
            //.WriteTo.File(Utils.Hosting.LogPath, Serilog.Events.LogEventLevel.Error,
            //rollingInterval: RollingInterval.Day,
            //rollOnFileSizeLimit: true)
            .CreateLogger();

        #region appLifetime Kill

        appLifetime.ApplicationStopping.Register(() =>
        {
            "Terminating application...".Log();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        });

        #endregion

        /* logClient
        var logClient = app.ApplicationServices.GetService<IHttpClientFactory>().CreateClient("log");
        logClient.BaseAddress = new Uri("http://localhost:5200/api");
        */

        app.CreateBusiness(logOptions =>
        {
            logOptions.Log = (type, message) =>
            {
                //Log interface
                switch (type)
                {
                    case LogType.Info:
                        Log.Information(message);
                        break;
                    case LogType.Error:
                        Log.Error(message);
                        break;
                    case LogType.Exception:
                        Log.Fatal(message);
                        break;
                }
            };
        })
        .UseDoc()
        .UseServer(server =>
        {
            //form
            server.FormOptions.KeyLengthLimit = int.MaxValue;
            server.FormOptions.ValueCountLimit = int.MaxValue;
            server.FormOptions.ValueLengthLimit = int.MaxValue;
            server.FormOptions.MultipartHeadersLengthLimit = int.MaxValue;
            server.FormOptions.MultipartBodyLengthLimit = long.MaxValue;
            server.FormOptions.MultipartBoundaryLengthLimit = int.MaxValue;

            //kestrel
            if (null != server.KestrelOptions)
            {
                server.KestrelOptions.Limits.MinRequestBodyDataRate = null;
                server.KestrelOptions.Limits.MinResponseDataRate = null;
                server.KestrelOptions.Limits.MaxConcurrentConnections = long.MaxValue;
                server.KestrelOptions.Limits.MaxConcurrentUpgradedConnections = long.MaxValue;
                server.KestrelOptions.Limits.MaxRequestBodySize = null;
            }
        })
        #region WebSocket
        .UseWebSocket<WebSocketManagement>().UseMessagePack(options => options.WithCompression(MessagePack.MessagePackCompression.Lz4Block))//Enable compressed transmission
        #endregion
        .UseJsonOptions((textJsonInOpt, textJsonOutOpt, newtonsoftJsonOpt) =>
        {
            //services.AddControllers().AddNewtonsoftJson(); You can choose NewtonsoftJson middleware!
        })
        .UseLogger(new Logger(async logs =>
        {
            //var result = await logs.Log(logClient);
            logs.JsonSerialize().Log();
        }, new Logger.BatchOptions
        {
            Interval = TimeSpan.FromSeconds(6),
            MaxNumber = 2000
        }))
        .Build();
    }
}
