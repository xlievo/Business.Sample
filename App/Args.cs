using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core.Annotations;
using Common;

/// <summary>
/// MyLogicArg!
/// </summary>
public struct MyLogicArg
{
    /// <summary>
    /// AAA
    /// </summary>
    [Common.CheckNull]
    public string A { get; set; }

    /// <summary>
    /// BBB
    /// </summary>
    public string B { get; set; }
}

/// <summary>
/// MyParameters!
/// </summary>
public class MyParametersArg
{
    /// <summary>
    /// AAA
    /// </summary>
    [Common.CheckNull]
    public string A { get; set; }

    /// <summary>
    /// BBB
    /// </summary>
    public string B { get; set; }
}

/// <summary>
/// WebSocketPushLogicData
/// </summary>
public struct WebSocketPushLogicData
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
