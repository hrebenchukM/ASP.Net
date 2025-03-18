using RequestProcessingPipeline;

namespace Request_Processing_Pipeline
{
    public static class SessionClearExtensions
    {
        public static IApplicationBuilder UseSessionClear(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionClearMiddleware>();
        }
    }
}
