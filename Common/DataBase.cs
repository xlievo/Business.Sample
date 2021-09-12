using Business.AspNet;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DataBase : Business.Data.DataBase<DataModel.Connection>
{
    public static readonly DataBase DB = new DataBase();//master

    //public static readonly DataBase DB2 = new DataBase("Slave");//slave

    static DataBase()
    {
        //Initialize the database
        LinqToDB.Data.DataConnection.DefaultSettings = new LinqToDBSection(Utils.Hosting.Config.GetSection("AppSettings").GetSection("ConnectionStrings").GetChildren().Select(c => new ConnectionStringSettings { Name = c.Key, ConnectionString = c.GetValue<string>("ConnectionString"), ProviderName = c.GetValue<string>("ProviderName") }));

        LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
        LinqToDB.Data.DataConnection.OnTrace = c =>
        {
            if (c.TraceInfoStep != LinqToDB.Data.TraceInfoStep.Completed)
            {
                if (c.TraceInfoStep == LinqToDB.Data.TraceInfoStep.Error)
                {
                    c.Exception?.Log();
                }
                return;
            }

            //var con = c.DataConnection as LinqToDB.LinqToDBConnection;

            //System.Console.WriteLine($"{c.StartTime}{con?.TraceMethod}:{con?.TraceId}{System.Environment.NewLine}{c.SqlText}{System.Environment.NewLine}{c.ExecutionTime}");
        };
    }

    readonly string configuration;

    internal protected DataBase(string configuration = null) => this.configuration = configuration;

    public override DataModel.Connection GetConnection([System.Runtime.CompilerServices.CallerMemberName] string callMethod = null) => new DataModel.Connection(this.configuration ?? LinqToDB.Data.DataConnection.DefaultSettings.DefaultConfiguration) { TraceMethod = callMethod };
}

#region Extensions

namespace DataModel
{
    /// <summary>
    /// Connection
    /// </summary>
    public partial class Connection : LinqToDBConnection
    {
        /// <summary>
        /// Connection
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="callMethod"></param>
        public Connection(string configuration, [System.Runtime.CompilerServices.CallerMemberName] string callMethod = null)
            : base(configuration) => TraceMethod = callMethod;
    }
}

/// <summary>
/// Paging
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Paging<T> : IPaging<T>
{
    /// <summary>
    /// Get paging data for
    /// </summary>
    public List<T> Data { get; set; }

    /// <summary>
    /// The length of the obtained paging data
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// The current paging index is determined by paging calculation
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total records
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int CountPage { get; set; }
}

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// GetPaging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageSizeMax"></param>
    /// <returns></returns>
    public static Paging<T> GetPaging<T>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50) => query.GetPaging<T, Paging<T>>(currentPage, pageSize, pageSizeMax);

    /// <summary>
    /// GetPagingAsync
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageSizeMax"></param>
    /// <returns></returns>
    public static ValueTask<Paging<T>> GetPagingAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50) => query.GetPagingAsync<T, Paging<T>>(currentPage, pageSize, pageSizeMax);

    /// <summary>
    /// GetPagingOrderBy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="query"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="keySelector"></param>
    /// <param name="order"></param>
    /// <param name="pageSizeMax"></param>
    /// <returns></returns>
    public static Paging<T> GetPagingOrderBy<T, TKey>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50) => query.GetPagingOrderBy<T, TKey, Paging<T>>(currentPage, pageSize, keySelector, order, pageSizeMax);

    /// <summary>
    /// GetPagingOrderByAsync
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="query"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="keySelector"></param>
    /// <param name="order"></param>
    /// <param name="pageSizeMax"></param>
    /// <returns></returns>
    public static ValueTask<Paging<T>> GetPagingOrderByAsync<T, TKey>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50) => query.GetPagingOrderByAsync<T, TKey, Paging<T>>(currentPage, pageSize, keySelector, order, pageSizeMax);
}

#endregion