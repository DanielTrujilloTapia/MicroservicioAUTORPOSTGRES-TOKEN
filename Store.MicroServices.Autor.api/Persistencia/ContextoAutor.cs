using Microsoft.EntityFrameworkCore;
using Store.MicroServices.Autor.api.Modelo;

namespace Store.MicroServices.Autor.api.Persistencia
{
    public class ContextoAutor : DbContext
    {
        public ContextoAutor(DbContextOptions<ContextoAutor> options) : base(options) { }
        public DbSet<AutorLibro> AutorLibros { get; set; }
        public DbSet<GradoAcademico> GradoAcademicos { get; set; }
    }
}
