namespace RequestProcessingPipeline
{
//    FromElevenToNineteenMiddleware:

//Обрабатывает числа от 11 до 19.
//Если число меньше 11 или больше 19, передает запрос следующему компоненту.
//Для чисел от 11 до 19, возвращает соответствующие текстовые значения, например, "Your number is twelve".

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
                if (number < 11 || number > 19)
                {
                    await _next.Invoke(context);  //Контекст запроса передаем следующему компоненту
                }
                else
                {
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is " + Numbers[number - 11]);
                }
            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
