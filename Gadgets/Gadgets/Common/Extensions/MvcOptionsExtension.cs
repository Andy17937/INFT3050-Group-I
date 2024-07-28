using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gadgets.Common.Filters;

namespace Gadgets.Common.Extensions
{
    /// <summary>
    /// 扩展了MVCoptions
    /// </summary>
    public static class MvcOptionsExtension
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routeAttribute">自定的前缀内容</param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // Add我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConventionFilter(routeAttribute));
        }
    }
}
