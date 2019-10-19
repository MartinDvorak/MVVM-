using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class ContributionUserTagRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public ContributionUserTagRepositoryTestsFixture() : base(nameof(ContributionUserTagRepositoryTestsFixture))
        {
            Repository = new ContributionUserTagRepository(DbContextFactory, new Mapper.Mapper());
            UserRepository = new UserRepository(DbContextFactory, new Mapper.Mapper());
            PostRepository = new PostRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public ContributionUserTagRepository Repository { get; }
        public UserRepository UserRepository { get; }
        public PostRepository PostRepository { get; }
    }
}
