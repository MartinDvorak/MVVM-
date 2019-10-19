using TeamsManager.DAL.DbContext;

namespace TeamsManager.DAL.Tests
{
    public class TeamsManagerDbContextTestsClassSetupFixture : TeamsManagerDbContextSetupFixture
    {
        public TeamsManagerDbContext TeamsManagerDbContextSUT { get; }
        public TeamsManagerDbContextTestsClassSetupFixture() : base(nameof(TeamsManagerDbContextTestsClassSetupFixture)) 
            => this.TeamsManagerDbContextSUT = DbContextFactory.CreateDbContext();
    }
}