using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;
        public VillaNumberRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public async Task<VillaNumber> Update(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            db.VillaNumbers.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }


    }
}
