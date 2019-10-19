using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class SearchResultsListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;

        public bool Visibility { get; set; } = false;
        public string Pattern { get; set; }
        public TeamModel SelectedTeam { get; set; }
        public ObservableCollection<PostModel> MatchedPosts { get; set; }
        public ObservableCollection<CommentModel> MatchedComments { get; set; }

        public SearchResultsListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            mediator.Register<SearchInPostsMessage>(SearchInPostsAssigned);
            mediator.Register<MyTeamSelectedMessage>(TeamSelected);

            //visibility handling
            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamInformationToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamMembersToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<AddPostMessage>(TurnVisibilityToFalse);
            mediator.Register<CreateNewTeamMessage>(TurnVisibilityToFalse);
            mediator.Register<ShowTeamPostsToggleVisibilityMessage>(TurnVisibilityToFalse);

            CloseSearchWindowCommand = new RelayCommand(CloseSearchWindow);
        }

        private void TurnVisibilityToFalse(IMessage obj)
        {
            Visibility = false;
        }

        private void TeamSelected(MyTeamSelectedMessage myTeamSelectedMessage)
        {
            TeamModel model = facade.GetDetail(myTeamSelectedMessage.MyTeam) as TeamModel;
            SelectedTeam = model;
        }

        private void SearchInPostsAssigned(SearchInPostsMessage searchInPostsMessage)
        {
            Visibility = true;
            Pattern = searchInPostsMessage.Pattern;

            IList<ContributionLightModel> matchedContributions = facade.Search(Pattern, SelectedTeam).ToList();

            MatchedPosts = new ObservableCollection<PostModel>();
            MatchedComments = new ObservableCollection<CommentModel>();

            //separate contributions to posts and comments
            foreach (ContributionLightModel lightModel in matchedContributions)
            {
                switch (lightModel)
                {
                    case PostLightModel postLightModel:
                        MatchedPosts.Add(facade.GetDetail(postLightModel) as PostModel);
                        break;
                    case CommentLightModel commentLightModel:
                        MatchedComments.Add(facade.GetDetail(commentLightModel) as CommentModel);
                        break;
                }
            }
        }

        public ICommand CloseSearchWindowCommand { get; set; }

        private void CloseSearchWindow()
        {
            Visibility = false;
            MatchedPosts = null;
            MatchedComments = null;
            Pattern = null;

            mediator.Send(new SearchWindowClosedMessage());
        }
    }
}
