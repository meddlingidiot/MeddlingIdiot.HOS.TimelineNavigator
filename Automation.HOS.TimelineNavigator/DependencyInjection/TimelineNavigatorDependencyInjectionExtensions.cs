using Microsoft.Extensions.DependencyInjection;

namespace Automation.HOS.TimelineNavigator.DependencyInjection
{
    public static class TimelineNavigatorDependencyInjectionExtensions
    {
        public static IServiceCollection AddTimelineNavigator(
            this IServiceCollection services)
        {
            services.AddTransient<ITimelineNavigator, TimelineNavigator>();

            return services;
        }
    }
}