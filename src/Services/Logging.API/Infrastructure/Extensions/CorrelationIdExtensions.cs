using EESLP.Services.Logging.API.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Infrastructure.Extensions
{
    public static class CorrelationIdExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AddCorrelationIdMiddleware>();
        }
    }
}
