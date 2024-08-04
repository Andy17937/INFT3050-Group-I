using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gadgets.Common.Helpers;
using Gadgets.Models;

namespace Gadgets.Common.Filters
{
    /// <summary>
    /// license middleware
    /// </summary>
    public class JwtAuthorizationFilter
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next"></param>
        public JwtAuthorizationFilter(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            //Detects if the 'Authorization' request header is included, if not then it is released directly
            //if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            if (!httpContext.Request.Headers.ContainsKey("Token"))
            {
                return _next(httpContext);
            }
            //var tokenHeader = httpContext.Request.Headers["Authorization"];
            var tokenHeader = httpContext.Request.Headers["Token"];

            try
            {
                tokenHeader = tokenHeader.ToString().Substring("Bearer ".Length).Trim();

                TokenModel tm = JwtHelper.SerializeJWT(tokenHeader);

                CacheHelper.TokenModel = tm;

                //authorizations
                var claimList = new List<Claim>();
                var claim = new Claim(ClaimTypes.Role, tm.Role);
                claimList.Add(claim);
                var identity = new ClaimsIdentity(claimList);
                var principal = new ClaimsPrincipal(identity);
                httpContext.User = principal;
            }
            catch(Exception ex)
            {

            }

            return _next(httpContext);
        }
    }
}
