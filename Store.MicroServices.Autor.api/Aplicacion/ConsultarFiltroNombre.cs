using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.MicroServices.Autor.api.Modelo;
using Store.MicroServices.Autor.api.Persistencia;

namespace Store.MicroServices.Autor.api.Aplicacion
{
    public class ConsultarPorNombre
    {
        public class AutorPorNombre : IRequest<List<AutorDto>>
        {
            public string Nombre { get; set; }
        }

        public class Manejador : IRequestHandler<AutorPorNombre, List<AutorDto>>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(AutorPorNombre request, CancellationToken cancellationToken)
            {
                //||a.Apellido.Contains(request.Nombre)
                var autores = await _context.AutorLibros.Where(a => a.Nombre.Contains(request.Nombre)).ToListAsync();

                if (autores == null || !autores.Any())
                {
                    throw new Exception("No se encontraron autores con ese nombre");
                }

                var autoresDto = _mapper.Map<List<AutorLibro>, List<AutorDto>>(autores);

                return autoresDto;
            }
        }
    }
}