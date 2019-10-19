using System;
using TeamsManager.DAL.DbContext;

namespace TeamsManager.DAL.Tests
{
    public class TeamsManagerDbContextSetupFixture : IDisposable
    {
        public InMemoryDbContextFactory DbContextFactory { get; }

        public TeamsManagerDbContextSetupFixture(string testDbName) => DbContextFactory = new InMemoryDbContextFactory(testDbName);

        public void PrepareDatabase()
        {
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }

        public void TearDownDatabase()
        {
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        public void Dispose()
        {
            TearDownDatabase();
        }
    }
}
