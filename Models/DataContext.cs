using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Models
{
    public class DataContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;uid=root;pwd=ThanhNh4n;database=discordbotdb;", new MySqlServerVersion(new System.Version(8,10,23)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            // User n - n League referencing
            modelBuilder.Entity<UserLeagueVouch>().HasKey(x => new { x.UserId, x.LeagueId });
            modelBuilder.Entity<UserLeagueVouch>()
                        .HasOne(x => x.User)
                        .WithMany(x => x.UserVouches)
                        .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserLeagueVouch>()
                        .HasOne(x => x.League)
                        .WithMany(x => x.UserVouches)
                        .HasForeignKey(x => x.LeagueId);
        }

        public DbSet<League> Leagues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLeagueVouch> UserLeagueVouches { get; set; }
    }
}
