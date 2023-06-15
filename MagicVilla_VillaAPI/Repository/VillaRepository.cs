using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext db;
        public VillaRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public async Task<Villa> Update(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            db.Villas.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
