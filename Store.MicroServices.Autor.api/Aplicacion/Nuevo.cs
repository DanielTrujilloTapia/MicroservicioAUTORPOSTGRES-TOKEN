using FluentValidation;
using MediatR;
using Store.MicroServices.Autor.api.Modelo;
using Store.MicroServices.Autor.api.Persistencia;

namespace Store.MicroServices.Autor.api.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; } 
            public string Apellido { get; set; }
            public DateTime FechaNacimiento { get; set; }
        }
        
        public class EjecutarValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutarValidacion() 
            {
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Apellido).NotEmpty();
                RuleFor(p => p.FechaNacimiento).NotEmpty();
            }
        }

        public class Manjeador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoAutor _context;

            public Manjeador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Forzar DateTime a UTC
                var fechaNacimientoUtc = request.FechaNacimiento.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(request.FechaNacimiento, DateTimeKind.Utc)
                    : request.FechaNacimiento.ToUniversalTime();

                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = fechaNacimientoUtc,
                    AutorLibroGuid = Guid.NewGuid().ToString()
                };

                _context.AutorLibros.Add(autorLibro);

                var respuesta = await _context.SaveChangesAsync();

                if (respuesta > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el autor del libro");
            }
        }

    }
}