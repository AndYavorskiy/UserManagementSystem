using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace UserManagementSystem.UI.Middlewares
{
    public sealed class GlobalErrorHandling
    {
        private readonly RequestDelegate next;

        public GlobalErrorHandling(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
