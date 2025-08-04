using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.MicroServices.Autor.api.Aplicacion;

namespace Store.MicroServices.Autor.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/autor
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        // GET: api/autor
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }

        // GET: api/autor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            return await _mediator.Send(new ConsultarFiltro.AutorUnico() { AutorGuid = id });
        }

        // GET: api/autor/buscar?nombre=Juan
        [HttpGet("buscar")]
        public async Task<ActionResult<List<AutorDto>>> BuscarPorNombre([FromQuery] string nombre)
        {
            return await _mediator.Send(new ConsultarPorNombre.AutorPorNombre { Nombre = nombre });
        }

        // PUT: api/autor/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(string id, Editar.EditarAutorCommand data)
        {
            if (id != data.AutorLibroGuid)
                return BadRequest("El ID de la URL no coincide con el del body");

            return await _mediator.Send(data);
        }

        // DELETE: api/autor/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(string id)
        {
            return await _mediator.Send(new Eliminar.EliminarAutorCommand { AutorLibroGuid = id });
        }
    }
}
