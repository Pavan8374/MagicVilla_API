using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Famous Villa",
                    Details = "vibrant Villa with swimming pool",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }, new Villa()
                {
                    Id = 2,
                    Name = "Famous Villa2",
                    Details = "vibrant Villa with swimming pool",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 100,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }, new Villa()
                {
                    Id = 3,
                    Name = "Famous Villa",
                    Details ="luxurious villa with swimming pool",
                    ImageUrl = "",
                    Occupancy = 6,
                    Rate = 250,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }
                ); ;
        }
    }
}
