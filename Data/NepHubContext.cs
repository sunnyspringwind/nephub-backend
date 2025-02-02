using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Models;

namespace NepHubAPI.Data
{
    public class NepHubContext : DbContext
    {
        public NepHubContext(DbContextOptions<NepHubContext> options) : base(options)
        {
        }

        public DbSet<Richest> Richest { get; set; }

        //ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //richest table customizations
         

            modelBuilder.Entity<Richest>()
                .HasData(
                    new Richest
                    {
                        Id = 1,
                        Name = "Binod Chaudhary",
                        Image = "https://sgp1.digitaloceanspaces.com/awe/publication-nepalaya/persons/binod_chaudhary.jpg",
                        Designation = "Chairman of Chaudhary Group",
                        NetWorth = "1.4 Billion USD"
                    },
                    new Richest
                    {
                        Id = 2,
                        Name = "Shesh Ghale",
                        Image = "https://republicaimg.nagariknewscdn.com/shared/web/uploads/media/shesh-ghale.jpg",
                        Designation = "CEO of Melbourne Institute of Technology",
                        NetWorth = "1.2 Billion USD"
                    },
                    new Richest
                    {
                        Id = 3,
                        Name = "Jamuna Gurung",
                        Image = "https://biographnepal.com/wp-content/uploads/2024/09/Jamuna-Gurung-e1727365992412.jpg",
                        Designation = "Co-founder of Melbourne Institute of Technology",
                        NetWorth = "1.2 Billion USD"
                    }
                );

        }
    }
}