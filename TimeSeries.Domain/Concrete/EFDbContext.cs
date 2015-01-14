using TimeSeries.Domain.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeSeries.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext<AppUser>
    {
        public EFDbContext()
            : base("EFDbContext")
        {
        }

        static EFDbContext()
        {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<TimeSerie> TimeSeries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new TimeSerieMap());
        }
        public static EFDbContext Create()
        {
            return new EFDbContext();
        }
    }
}
