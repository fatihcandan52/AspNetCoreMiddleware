using AspNetCoreMiddleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCoreMiddleware.Middlewares
{
    public class WhiteIPControl
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;

        public WhiteIPControl(RequestDelegate next, IOptions<IPList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestIP = httpContext.Connection.RemoteIpAddress;

            var ipCheck = _ipList.WhiteList.Contains(requestIP.ToString());

            if (!ipCheck)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(httpContext);
        }
    }
}
