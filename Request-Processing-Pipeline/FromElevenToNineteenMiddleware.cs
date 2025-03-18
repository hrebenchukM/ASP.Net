using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace RequestProcessingPipeline
{
    //    FromElevenToNineteenMiddleware:


    public class FromElevenToNineteenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromElevenToNineteenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number % 100 < 11 || number % 100 > 19 )
                {
                    await _next.Invoke(context);  //Контекст запроса передаем следующему компоненту
                }
                else if (number > 100 && number % 100 >= 11 && number % 100 <= 19)
                {
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    context.Session.SetString("numbers", Numbers[number % 100 - 11]);

                }
                else
                {
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    context.Session.SetString("numbers", Numbers[number - 11]);
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is " + Numbers[number - 11]);
                }
            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter FromElevenToNineteenMiddleware");
            }
        }
    }
}