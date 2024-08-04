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
    /// Extended MVCoptions
    /// </summary>
    public static class MvcOptionsExtension
    {
        /// <summary>
        /// Extended Methods
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="routeAttribute">Customized prefix content</param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // Add our custom RouteConvention that implements IApplicationModelConvention.
            opts.Conventions.Insert(0, new RouteConventionFilter(routeAttribute));
        }
    }
}
