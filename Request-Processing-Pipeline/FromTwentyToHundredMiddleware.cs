namespace RequestProcessingPipeline
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;
        public FromTwentyToHundredMiddleware(RequestDelegate next)//сюда приходит ссылка на следующий middleware компонент в конвеере. 
        {
            this._next = next;//сохраняется в классе
        }
        //То есть в цепочке компонентов, этот знает про следующий и может передать эстафетную палочку дальше
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"]; // Получим число из контекста запроса
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 20)
                {
                    await _next.Invoke(context); //Контекст запроса передаем следующему компоненту
                }
                else if(number > 100)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Number greater than one hundred");
                }
                else if (number == 100)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is one hundred");
                }
                else
                {
                    string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    if (number % 10 == 0)
                    {
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]); 
                    }
                    else
                    { 
                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту.Будет ждать пока второй компанент не передаст ему управление await отпустит поток то первый компонент делает следующую логику.
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); // получим число от компонента FromOneToTenMiddleware
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2] + " " + result);
                    }                   
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
