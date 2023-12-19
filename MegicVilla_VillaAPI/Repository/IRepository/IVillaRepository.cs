using MegicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MegicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaRepository : IRepository<Villa>
	{
		Task<Villa> UpdateAsync(Villa entity);
	}
}
