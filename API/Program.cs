using Microsoft.Extensions.DependencyInjection; // Asegúrate de importar este espacio de nombres
using API.Attributes;
using API.Extensions;
using DAC;
using Microsoft.AspNetCore.Mvc;
using REPO.Extensions;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureCors();

            builder.Services.AddControllers(options => 
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.ConfigureSwagger();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.ConfigureREPOExtensions(builder.Configuration);

            builder.Services.AddScoped<ValidationFilterAttribute>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    await SeedData.InsertSeedDataAsync(context, loggerFactory);
                }
                catch (Exception ex)
                {
                    var loggerException = loggerFactory.CreateLogger<Program>();
                    loggerException.LogError(ex, ex.Message);
                }
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
