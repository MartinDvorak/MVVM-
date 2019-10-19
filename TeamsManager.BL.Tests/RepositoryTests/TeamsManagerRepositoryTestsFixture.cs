using TeamsManager.BL.Facade;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class TeamsManagerRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public IRepository<UserModel, UserLightModel> UserRepository { get; }
        public IRepository<ProfileImageModel, ProfileImageLightModel> ProfileImageRepository { get; }
        public IRepository<CommentModel, CommentLightModel> CommentRepository { get; }
        public IRepository<ContributionFileModel, ContributionFileLightModel> ContributionFileRepository { get; }
        public IRepository<PostModel, PostLightModel> PostRepository { get; }
        public IRepository<TeamModel, TeamLightModel> TeamRepository { get; }
        public IRelationRepository<UserTeamMemberModel> UserTeamMemberRepository { get; }
        public IRelationRepository<ContributionUserTagModel> ContributionUserTagRepository { get; }
        public IBusinessLogicFacade Facade { get; }

        public TeamsManagerRepositoryTestsFixture() : base(nameof(TeamsManagerRepositoryTestsFixture))
        {

            UserRepository = new UserRepository(DbContextFactory, new Mapper.Mapper());
            ProfileImageRepository = new ProfileImageRepository(DbContextFactory, new Mapper.Mapper());
            CommentRepository = new CommentRepository(DbContextFactory, new Mapper.Mapper());
            ContributionFileRepository = new ContributionFileRepository(DbContextFactory, new Mapper.Mapper());
            PostRepository = new PostRepository(DbContextFactory, new Mapper.Mapper());
            TeamRepository = new TeamRepository(DbContextFactory, new Mapper.Mapper());
            UserTeamMemberRepository = new UserTeamMemberRepository(DbContextFactory, new Mapper.Mapper());
            ContributionUserTagRepository = new ContributionUserTagRepository(DbContextFactory, new Mapper.Mapper());

            Facade = new BusinessLogicFacade(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
            
        }
    }
}
