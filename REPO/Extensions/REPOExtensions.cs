using DAC.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REPO.Interfaces;
using REPO.Repositories;

namespace REPO.Extensions
{
    public static class REPOExtensions
    {
        public static void ConfigureREPOExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDACExtensions(configuration);

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        }
    }
}
