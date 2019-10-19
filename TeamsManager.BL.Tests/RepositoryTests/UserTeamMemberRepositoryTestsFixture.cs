using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class UserTeamMemberRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public UserTeamMemberRepositoryTestsFixture() : base(nameof(UserTeamMemberRepositoryTestsFixture))
        {
            Repository = new UserTeamMemberRepository(DbContextFactory, new Mapper.Mapper());
            UserRepository = new UserRepository(DbContextFactory, new Mapper.Mapper());
            TeamRepository = new TeamRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }
        public UserTeamMemberRepository Repository { get; }
        public UserRepository UserRepository { get; }
        public TeamRepository TeamRepository { get; }

    }
}
