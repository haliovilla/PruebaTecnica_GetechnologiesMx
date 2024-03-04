using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAC.Extensions
{
    public static class DACExtensions
    {
        public static void ConfigureDACExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("AppDatabase");
            });
        }
    }
}
