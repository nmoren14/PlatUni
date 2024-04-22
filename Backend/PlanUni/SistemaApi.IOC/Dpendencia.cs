using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaApi.BLL.Servicios;
using SistemaApi.BLL.Servicios.Contrato;
using SistemaApi.DAL.DBContext;
using SistemaApi.DAL.Repositorios;
using SistemaApi.DAL.Repositorios.Contrato;
using SistemaUni.Util;

namespace SistemaApi.IOC
{
    public static class Dpendencia
    {
        public static void InyectarDeoendencias(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<PlatUniContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
                    });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStudentRepository,StudentRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IProfessorService, ProfessorService>();
        }
    }
}
