using Tourism.Domain.Entities;
using System.Data.Entity;

namespace Tourism.Domain.Concrete {
    public class EFDbContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
    }
}