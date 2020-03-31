using Microsoft.EntityFrameworkCore;
using Recruitment.Core;

namespace Recruitment.Data
{
    public class RecruitmentDbContext : DbContext
    {
        public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options)
        {
        }

        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<HrMember> HrMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobOffer>().HasData(
                new JobOffer(100, "Server Manager", "You need to be the king in the router magic"),
                new JobOffer(101, "Account Manager", "You need to be able to handle phone calls and making coffee")
            );

            modelBuilder.Entity<Application>().HasData(
                new Application(100, 100, "ServerManger", "Andrew Dudes", "tomterka1@gmail.com", "tomterka1@gmail.com",
                    "132123123", "info", "nofile")
            );

            modelBuilder.Entity<Application>().HasIndex(p => new {p.OfferName});
        }
    }
}