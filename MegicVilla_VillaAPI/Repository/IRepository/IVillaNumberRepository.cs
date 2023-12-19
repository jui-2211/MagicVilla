using MegicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MegicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaNumberRepository : IRepository<VillaNumber>
	{
		Task<VillaNumber> UpdateAsync(VillaNumber entity);
	}
}
