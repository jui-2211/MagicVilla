using MegicVilla_VillaAPI.Data;
using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace MegicVilla_VillaAPI.Repository
{
	public class VillaRepository : Repository<Villa>, IVillaRepository
	{
		private readonly ApplicationDbContext _db;

		public VillaRepository(ApplicationDbContext db) : base (db) 
		{
			_db = db;
		}
	
		public async Task<Villa> UpdateAsync(Villa entity)
		{
			entity.UpdatedDate = DateTime.Now;
			_db.Villas.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}

	}
}
