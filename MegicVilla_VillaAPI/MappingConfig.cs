using AutoMapper;
using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Models.Dto;

namespace MegicVilla_VillaAPI
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<Villa, VillaDTO>();

			CreateMap<VillaDTO, Villa>();

			CreateMap<Villa, VillaCreateDTO>().ReverseMap();

			CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

			CreateMap<VillaNumber, VillaNumberDTO>();

			CreateMap<VillaNumberDTO, VillaNumber>();

			CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();

			CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
		}
	}
}
