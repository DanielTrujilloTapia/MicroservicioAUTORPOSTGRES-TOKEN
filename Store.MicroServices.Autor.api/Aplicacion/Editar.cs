using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.MicroServices.Autor.api.Modelo;
using Store.MicroServices.Autor.api.Persistencia;

namespace Store.MicroServices.Autor.api.Aplicacion
{
    public class Editar
    {
        public class EditarAutorCommand : IRequest
        {
            public string AutorLibroGuid { get; set; } 
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; } 
        }

        public class EjecutaValidacion : AbstractValidator<EditarAutorCommand>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.AutorLibroGuid).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                // La fecha puede ser opcional
            }
        }

        public class Manejador : IRequestHandler<EditarAutorCommand>
        {
            private readonly ContextoAutor _context;

            public Manejador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EditarAutorCommand request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros
                    .FirstOrDefaultAsync(x => x.AutorLibroGuid == request.AutorLibroGuid);

                if (autor == null)
                {
                    throw new Exception("El autor no fue encontrado");
                }

                autor.Nombre = request.Nombre ?? autor.Nombre;
                autor.Apellido = request.Apellido ?? autor.Apellido;

                if (request.FechaNacimiento.HasValue)
                {
                    var fechaUtc = request.FechaNacimiento.Value.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(request.FechaNacimiento.Value, DateTimeKind.Utc)
                        : request.FechaNacimiento.Value.ToUniversalTime();

                    autor.FechaNacimiento = fechaUtc;
                }

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios del autor");
            }
        }

    }
}
