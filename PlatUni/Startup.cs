using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PlatUni
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Obtener la cadena de conexión desde la configuración (appsettings.json)
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Registrar el contexto de base de datos con Entity Framework Core
            services.AddDbContext<PlatUniContext>(options =>
                options.UseSqlServer(connectionString)
            );
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }

}
