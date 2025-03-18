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
                        string? resultTens = context.Session.GetString("tens");
                        string? resultOnes = context.Session.GetString("ones");
                        string? resultNumbers = context.Session.GetString("numbers");

                        // Формируем строку с проверкой наличия значений
                        string result = Hundreds[number / 100 - 1];

                        if (!string.IsNullOrWhiteSpace(resultTens))
                            result += " " + resultTens;

                        if (!string.IsNullOrWhiteSpace(resultOnes))
                            result += " " + resultOnes;

                        if (!string.IsNullOrWhiteSpace(resultNumbers))
                            result += " " + resultNumbers;

                        await context.Response.WriteAsync("Your number is " + result);
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


