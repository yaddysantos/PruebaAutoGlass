using AutoGlassBack.DTO;
using AutoGlassBack.Models;
using AutoMapper;

namespace AutoGlassBack.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            ///Configuracion del mapeo automatico
            CreateMap<Producto, ProductoDTO>();
        }
    }
}
