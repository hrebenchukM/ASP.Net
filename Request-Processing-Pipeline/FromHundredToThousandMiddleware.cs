namespace Request_Processing_Pipeline
{
    //обрабатывать числа от 100 до 999.
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromHundredToThousandMiddleware(RequestDelegate next)
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


                if (number < 100)
                {
                    await _next.Invoke(context); //Контекст запроса передаем следующему компоненту
                }
                else if (number == 1000)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is one thousand");
                }
                else
                {
                    string[] Hundreds = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred",
                                               "six hundred", "seven hundred", "eight hundred", "nine hundred" };

                    if (number % 100 == 0)
                    {
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Hundreds[number / 100 - 1]);// Индексация с 0
                    }
                    else
                    {

                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту.Будет ждать пока второй компанент не передаст ему управление await отпустит поток то первый компонент делает следующую логику.
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); // получим число от компонента FromTwentyToHundredMiddleware
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Hundreds[number / 100 - 1] + " " + result);
                    }
                }


            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter FromHundredToThousandMiddleware");
            }
        }
    }
}


