using MegicVilla_VillaAPI.Models.Dto;

namespace MegicVilla_VillaAPI.Data
{
	public static class VillaStore
	{
		public static List<VillaDTO> villList = new List<VillaDTO>
			{
				new VillaDTO {Id = 1, Name ="Pool View",Sqft = 100,Occupancy = 4},
				new VillaDTO {Id = 2, Name ="Beach View",Sqft = 200,Occupancy = 6},
				new VillaDTO {Id = 3, Name ="Garden View",Sqft = 100,Occupancy = 2}
			};
	}
}
