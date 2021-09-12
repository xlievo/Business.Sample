using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// JsonArgAttribute
    /// </summary>
    public class JsonArgAttribute : Business.AspNet.JsonArgAttribute
    {
        /// <summary>
        /// JsonArgAttribute
        /// </summary>
        public JsonArgAttribute() : base() => Description = "Json format!";
    }

    /// <summary>
    /// NewtonsoftJsonArg
    /// </summary>
    public class NewtonsoftJsonArgAttribute : Business.AspNet.NewtonsoftJsonArgAttribute
    {
        /// <summary>
        /// NewtonsoftJsonArg
        /// </summary>
        public NewtonsoftJsonArgAttribute() : base() => Description = "Newtonsoft Json format!";
    }

    /// <summary>
    /// CheckNullAttribute
    /// </summary>
    public class CheckNullAttribute : Business.Core.Annotations.CheckNullAttribute
    {
        /// <summary>
        /// CheckNullAttribute
        /// </summary>
        public CheckNullAttribute() : base() => Description = "{Alias} is not allowed to be empty!";
    }

    /// <summary>
    /// SizeAttribute
    /// </summary>
    public class SizeAttribute : Business.Core.Annotations.SizeAttribute
    {
        /// <summary>
        /// SizeAttribute
        /// </summary>
        public SizeAttribute() : base()
        {
            this.BindAfter += () =>
            {
                if (null != Min && null != Max)
                {
                    Description = $"Input size limit [min {Min} - max {Max}]";
                }
                else if (null != Min)
                {
                    Description = $"Input size limit [min {Min}]";
                }
                else if (null != Max)
                {
                    Description = $"Input size limit [max {Max}]";
                }
            };
        }
    }
}
