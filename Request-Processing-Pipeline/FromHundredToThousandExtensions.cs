
namespace Request_Processing_Pipeline
{
    public static class FromHundredToThousandExtensions
    {
        public static IApplicationBuilder UseFromHundredToThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromHundredToThousandMiddleware>();
        }
    }
}
