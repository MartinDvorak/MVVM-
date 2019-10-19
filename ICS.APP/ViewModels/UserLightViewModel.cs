using System;
using System.Windows.Controls;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Services;
using System.Drawing;
using TeamsManager.BL.Model;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.APP.ViewModels
{
    public class UserLightViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;

        private bool userDetailVisible = false;
        public String UserDetailOverviewSwitchButtonText { get; set; } = "Edit Account";
        public UserLightModel Model { get; set; } = new UserLightModel();

        public UserLightViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.facade = facade;
            this.messageBoxService = messageBoxService;
            this.mediator = mediator;

            UserDetailToggleVisibilityCommand = new RelayCommand<UserLightModel>(UserDetailToggleVisibility);

            mediator.Register<AccountUpdatedMessage>(AccountUpdated);
            mediator.Register<UserAuthenticatedMessage>(LoadUser);
        }

        private void LoadUser(UserAuthenticatedMessage loggedUser)
        {
            Model = loggedUser.User;
        }

        private void UserDetailToggleVisibility(UserLightModel userLightModel)
        {
            mediator.Send(new UserDetailToggleVisibilityMessage {UserLightModel = userLightModel});

            userDetailVisible = !userDetailVisible;
            UserDetailOverviewSwitchButtonText = (userDetailVisible) ? "Show Overview" : "Edit Account";
        }

        public ICommand UserDetailToggleVisibilityCommand { get; }

        private void AccountUpdated(AccountUpdatedMessage accountUpdatedMessage)
        {
            Model = accountUpdatedMessage.UserLightModel;
        }
    }
}
