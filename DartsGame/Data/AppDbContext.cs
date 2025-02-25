using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Leg> Legs { get; set; }
        public DbSet<LegScore> LegScores { get; set; }
        public DbSet<Match> Matches { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerMatch> PlayerMatches { get; set; }

        public DbSet<Set> Sets { get; set; }

        public DbSet<SetResult> SetResults { get; set; }

        public DbSet<Turn> Turns { get; set; }

        public DbSet<TurnThrow> TurnThrows { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
           .HasMany(p => p.PlayerMatches)
           .WithOne(pm => pm.Player)
           .HasForeignKey(pm => pm.PlayerId);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.PlayerMatches)
                .WithOne(pm => pm.Match)
                .HasForeignKey(pm => pm.MatchId);

            modelBuilder.Entity<Match>()
                .HasMany(m => m.Sets)
                .WithOne(s => s.Match)
                .HasForeignKey(s => s.MatchId);

            modelBuilder.Entity<PlayerMatch>()
                .HasKey(pm => new { pm.PlayerId, pm.MatchId });

            modelBuilder.Entity<Set>()
                .HasMany(s => s.Legs)
                .WithOne(l => l.Set)
                .HasForeignKey(l => l.SetId);

            modelBuilder.Entity<Set>()
                .HasMany(s => s.SetResults)
                .WithOne(sr => sr.Set)
                .HasForeignKey(sr => sr.SetId);

            modelBuilder.Entity<SetResult>()
                .HasKey(sr => new { sr.SetId, sr.PlayerId });

            modelBuilder.Entity<SetResult>()
                .HasOne(sr => sr.Player)
                .WithMany()
                .HasForeignKey(sr => sr.PlayerId);

            modelBuilder.Entity<Leg>()
                .HasOne(l => l.Winner)
                .WithMany()
                .HasForeignKey(l => l.WinnerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Leg>()
                .HasMany(l => l.LegScores)
                .WithOne(ls => ls.Leg)
                .HasForeignKey(ls => ls.LegId)
                .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Leg>()
                .HasMany(l => l.Turns)
                .WithOne(t => t.Leg)
                .HasForeignKey(t => t.LegId);

            modelBuilder.Entity<LegScore>()
                .HasKey(ls => new { ls.LegId, ls.PlayerId });

            modelBuilder.Entity<LegScore>()
                .HasOne(ls => ls.Player)
                .WithMany()
                .HasForeignKey(ls => ls.PlayerId)
                .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Turn>()
                .HasOne(t => t.Player)
                .WithMany()
                .HasForeignKey(t => t.PlayerId);

            modelBuilder.Entity<TurnThrow>()
            .HasKey(tt => tt.TurnThrowId);  

            modelBuilder.Entity<TurnThrow>()
                .HasOne(tt => tt.Turn)
                .WithMany(t => t.TurnThrows)  
                .HasForeignKey(tt => tt.TurnId);
        }

    }
}
