using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.MicroServices.Autor.api.Persistencia;

namespace Store.MicroServices.Autor.api.Aplicacion
{
    public class Eliminar
    {
        public class EliminarAutorCommand : IRequest
        {
            public string AutorLibroGuid { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarAutorCommand>
        {
            private readonly ContextoAutor _context;

            public Manejador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EliminarAutorCommand request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros
                    .FirstOrDefaultAsync(x => x.AutorLibroGuid == request.AutorLibroGuid);

                if (autor == null)
                {
                    throw new Exception("No se encontró el autor");
                }

                _context.AutorLibros.Remove(autor);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el autor");
            }
        }
    }
}
