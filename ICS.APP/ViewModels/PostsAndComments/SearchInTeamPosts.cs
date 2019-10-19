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
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class SearchInTeamPosts : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;
        public string Pattern { get; set; } = "Write pattern here";
        public ICommand SearchInPostsCommand { get; set; }
        public SearchInTeamPosts(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            SearchInPostsCommand = new RelayCommand(findMatches,canFindMatches);

        }

        private bool canFindMatches(object arg)
        {
            return !string.IsNullOrWhiteSpace(this.Pattern);
        }

        private void findMatches(object obj)
        {
            mediator.Send(new SearchInPostsMessage {Pattern = this.Pattern});
        }
    }
}
