using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsManager.APP.Services;
using TeamsManager.BL.Extensions;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class UserRecentActivityViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;

        private UserLightModel loggedUser { get; set; }
        public bool IsLoggedUser { get; set; } = false;
        public UserRecentActivityViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.facade = facade;
            this.messageBoxService = messageBoxService;
            this.mediator = mediator;
            
            mediator.Register<UserAuthenticatedMessage>(LoadLoggedUser);
            mediator.Register<PostAddedMessage>(ReloadContent);
            mediator.Register<CommentAddedMessage>(ReloadContent);
            mediator.Register<PostOrCommentDeletedMessage>(ReloadContent);
        }
        
        private void ReloadContent(object obj)
        {
            LoadActivity();
        }

        private void LoadLoggedUser(UserAuthenticatedMessage loggedUserMessage)
        {
            this.loggedUser = loggedUserMessage.User;
            this.IsLoggedUser = true;
            LoadActivity();
        }

        public ObservableCollection<MyRecentActivityModel> MyRecentActivityCollection { get; set; } = new ObservableCollection<MyRecentActivityModel>();

        private void LoadActivity()
        {
            MyRecentActivityCollection.Clear();
            var myTeams = facade.GetRecentUserActivity(this.loggedUser)
                .ToList();
            MyRecentActivityCollection.AddRange(myTeams);
            facade.GetRecentUserActivity(this.loggedUser);
        }
    }

}
