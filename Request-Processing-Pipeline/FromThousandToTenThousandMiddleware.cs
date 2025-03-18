namespace Request_Processing_Pipeline
{  //обрабатывать числа от 1000 до 9999.
    public class FromThousandToTenThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromThousandToTenThousandMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        //То есть в цепочке компонентов, этот знает про следующий и может передать эстафетную палочку дальше
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);


                if (number < 1000)
                {
                    await _next.Invoke(context); //Контекст запроса передаем следующему компоненту
                }
                else if (number == 10000)
                {
                    // Выдаем окончательный ответ клиенту
                    await context.Response.WriteAsync("Your number is ten thousand");
                }
                else
                {
                    string[] Thousands = { "one thousand", "two thousand", "three thousand", "four thousand", "five thousand",
                                               "six thousand", "seven thousand", "eight thousand", "nine thousand" };

                    if (number % 100 == 0)
                    {
                        // Выдаем окончательный ответ клиенту
                        await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1]);// Индексация с 0
                    }
                    else
                    {

                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту.Будет ждать пока второй компанент не передаст ему управление await отпустит поток то первый компонент делает следующую логику.

                        string? resultHundreds = context.Session.GetString("hundreds"); 
                        string? resultTens = context.Session.GetString("tens");
                        string? resultOnes = context.Session.GetString("ones");
                        string? resultNumbers = context.Session.GetString("numbers");

                        // Формируем строку с проверкой наличия значений
                        string result = Thousands[number / 1000 - 1];
                        if (!string.IsNullOrWhiteSpace(resultHundreds))
                            result += " " + resultHundreds;

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
