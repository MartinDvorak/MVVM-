using TeamsManager.APP.Services;
using TeamsManager.APP.ViewModels.PostsAndComments;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model;
using TeamsManager.BL.Services;
using TeamsManager.DAL.DbContext;

namespace TeamsManager.APP.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IBusinessLogicFacade facade;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly IMessageBoxService messageBoxService;

        public MyTeamsListViewModel MyTeamsListViewModel => new MyTeamsListViewModel(facade,mediator);
        public TeamDetailViewModel TeamDetailViewModel => new TeamDetailViewModel(facade, messageBoxService, mediator);
        public TeamsMembersListViewModel TeamsMembersListViewModel => new TeamsMembersListViewModel(facade, mediator);
        public TeamsNonMembersListViewModel TeamsNonMembersListViewModel => new TeamsNonMembersListViewModel(facade,mediator);
        public UserDetailViewModel UserDetailViewModel => new UserDetailViewModel(facade, messageBoxService , mediator);
        public TeamsNewestPostsListViewModel TeamsNewestPostsListViewModel => new TeamsNewestPostsListViewModel(facade, mediator);
        public PostDetailViewModel PostDetailViewModel => new PostDetailViewModel(facade, messageBoxService, mediator);
        public CommentInTeamsDetailViewModel CommentInTeamsDetailViewModel => new CommentInTeamsDetailViewModel(facade, messageBoxService, mediator);
        public CommentInUsersDetailViewModel CommentInUsersDetailViewModel => new CommentInUsersDetailViewModel(facade, messageBoxService, mediator);
        public UserLightViewModel UserLightViewModel => new UserLightViewModel(facade, messageBoxService, mediator);
        public UsersNewestPostsListViewModel UsersNewestPostsListViewModel => new UsersNewestPostsListViewModel(facade, mediator);
        public UsersTagsListViewModel UsersTagsListViewModel => new UsersTagsListViewModel(facade, mediator);
        public LoginViewModel LoginViewModel => new LoginViewModel(facade, mediator);
        public MainViewModel MainViewModel => new MainViewModel(mediator);
        public CreateAccountViewModel CreateAccountViewModel => new CreateAccountViewModel(facade, mediator);
        public UserRecentActivityViewModel UserRecentActivityViewModel => new UserRecentActivityViewModel(facade,messageBoxService,mediator);
        public SearchInTeamPosts SearchInTeamPosts => new SearchInTeamPosts(facade,mediator);
        public SearchResultsListViewModel SearchResultsListViewModel => new SearchResultsListViewModel(facade, mediator);

        public ViewModelLocator()
        {
            mapper = new Mapper();
            dbContextFactory = new DefaultDbContextFactory();
            facade = new BusinessLogicFacade(dbContextFactory, mapper);
            messageBoxService = new MessageBoxService();
            mediator = new Mediator();
        }
    }
}
