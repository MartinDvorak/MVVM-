using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class TeamRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public TeamRepositoryTestsFixture() : base(nameof(TeamRepositoryTestsFixture))
        {
            Repository = new TeamRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public TeamRepository Repository { get; }
    }
}
