using AutoMapper;
using Store.MicroServices.Autor.api.Modelo;

namespace Store.MicroServices.Autor.api.Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }
    }
}
