namespace RequestProcessingPipeline
{
//    FromOneToTenMiddleware:
    public class FromOneToTenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            string? token = context.Request.Query["number"]; // Получим число из контекста запроса
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if(number == 10)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is ten"); // 
                }
                else
                {
                    string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                    // Любые числа больше 20, но не кратные 10
                    if (number > 20 && number % 10 != 0)
                    {
                        // Записываем в сессионную переменную number результат для компонента FromTwentyToHundredMiddleware
                        context.Session.SetString("ones", Ones[number % 10 - 1]);  //в числе 25 тут число пять записали в сессию
                        string? result = string.Empty;
                        result = context.Session.GetString("ones");// получим число от компонента FromOneToTenMiddleware
                      

                    }
                    else if (number >= 1 && number <= 9)
                    {
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Ones[number - 1]); // от 1 до 9

                    }
                }            
            }
            catch(Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter FromOneToTenMiddleware");
            }
        }
    }
}
