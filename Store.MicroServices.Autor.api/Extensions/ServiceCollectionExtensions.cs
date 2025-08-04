using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.MicroServices.Autor.api.Persistencia;
using Store.MicroServices.Autor.api.Aplicacion;

namespace Store.MicroServices.Autor.api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            services.AddDbContext<ContextoAutor>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddMediatR(typeof(Nuevo.Manjeador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));

            return services;
        }
    }
}
