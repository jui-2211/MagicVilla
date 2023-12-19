using MegicVilla_VillaAPI.Data;
using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace MegicVilla_VillaAPI.Repository
{
	public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
	{
		private readonly ApplicationDbContext _db;

		public VillaNumberRepository(ApplicationDbContext db) : base (db) 
		{
			_db = db;
		}
	
		public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
		{
			entity.UpdatedDate = DateTime.Now;
			_db.VillaNumbers.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}

	}
}
