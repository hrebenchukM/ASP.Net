using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Request_Processing_Pipeline
{
   
    public class SessionClearMiddleware
    {
        private readonly RequestDelegate _next;


        public SessionClearMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Session.Clear();
            await _next(context);
        }

    }
    

}
