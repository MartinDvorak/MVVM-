using Microsoft.EntityFrameworkCore;
using TeamsManager.DAL.Entities;
using TeamsManager.DAL.Entities.Contributions;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.DAL.DbContext
{
    public class TeamsManagerDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TeamsManagerDbContext()
        {
        }
        public TeamsManagerDbContext(DbContextOptions<TeamsManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ContributionFile> ContributionFiles { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<ContributionUserTag> ContributionUserTags { get; set; }
        public DbSet<UserTeamMember> UserTeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Photo);
                //.WithOne()
                //.OnDelete(DeleteBehavior.Cascade);                ;
                   
                //.WithOne(b => b.AssociatedUser)
                //.HasForeignKey<ProfileImage>(b => b.AssociatedUserID);

            modelBuilder.Entity<ContributionUserTag>()
                .HasKey(cut => new { cut.UserId, cut.ContributionId });
            modelBuilder.Entity<ContributionUserTag>()
                .HasOne(cut => cut.User)
                .WithMany(u => u.ContributionUserTags)
                .HasForeignKey(cut => cut.UserId);
            modelBuilder.Entity<ContributionUserTag>()
                .HasOne(cut => cut.Contribution)
                .WithMany(c => c.ContributionUserTags)
                .HasForeignKey(cut => cut.ContributionId);

            modelBuilder.Entity<UserTeamMember>()
                .HasKey(utm => new { utm.UserId, utm.TeamId });
            modelBuilder.Entity<UserTeamMember>()
                .HasOne(utm => utm.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(utm => utm.UserId);
            modelBuilder.Entity<UserTeamMember>()
                .HasOne(utm => utm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(utm => utm.TeamId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentContribution)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.ParentContribution);


        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer($@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = MigrationDb;MultipleActiveResultSets = True;Integrated Security = True; ");
        //    }

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
    
}
