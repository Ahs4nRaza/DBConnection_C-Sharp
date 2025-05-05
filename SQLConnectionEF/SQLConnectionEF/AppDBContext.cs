using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace SQLConnectionEF
{
    public class AppDbContext : DbContext
    {
        
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<LeagueTeam> LeagueTeams { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("YOUR CONNECTION STRING HERE");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        // Configure the relationships in the model 
            modelBuilder.Entity<LeagueTeam>()
                .HasKey(lt => new { lt.LeagueId, lt.TeamId });

            modelBuilder.Entity<LeagueTeam>()
                .HasOne(lt => lt.League)
                .WithMany(l => l.LeagueTeams)
                .HasForeignKey(lt => lt.LeagueId);

            modelBuilder.Entity<LeagueTeam>()
                .HasOne(lt => lt.Team)
                .WithMany(t => t.LeagueTeams)
                .HasForeignKey(lt => lt.TeamId);
        }
    }
}

/*
  if we use ef6+ we can do following to omit out association table configuration for keys:
    public class AppDbContext : DbContext
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TestDB;Trusted_Connection=True;");
        }
    }
 */