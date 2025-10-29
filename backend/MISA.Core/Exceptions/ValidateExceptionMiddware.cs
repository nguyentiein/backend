using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Exceptions
{
    public class ValidateExceptionMiddware
    {
        private readonly RequestDelegate _next;
        public ValidateExceptionMiddware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ValidateException ex)
            {
                context.Response.StatusCode = 400;
                var res = new
                {
                    DevMsg = "Lỗi Validate",
                    UserMsg = ex.Message
                };

                var resJson = System.Text.Json.JsonSerializer.Serialize(res);
                await context.Response.WriteAsync(resJson);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
