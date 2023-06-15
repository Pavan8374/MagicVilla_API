using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>(); 
            CreateMap<VillaDTO, Villa>(); 
            CreateMap<VillaCreateDTO, Villa>().ReverseMap(); 
 
            CreateMap<VillaUpdateDTO, Villa>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>();
            CreateMap<VillaNumberDTO, VillaNumber>();
            CreateMap<VillaNumberCreateDTO, VillaNumber>().ReverseMap();

            CreateMap<VillaNumberUpdateDTO, VillaNumber>().ReverseMap();

        }
    }
}
