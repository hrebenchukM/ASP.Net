namespace RequestProcessingPipeline
{
    public static class FromTwentyToHundredExtensions
    {//метод расширения добавляет метод/ расширяет класс IApplicationBuilder но без наследования.
        public static IApplicationBuilder UseFromTwentyToHundred(this IApplicationBuilder builder)//IApplicationBuilder этот обьект реализующий интерфейс и представляет собой middleware конвеер 
        {
            return builder.UseMiddleware<FromTwentyToHundredMiddleware>();//добавляем в middleware конвеер свой собственный компонент.Вызывая обощенный метод UseMiddleware и типизируя этот метод классом компонента 
        }
    }
}
