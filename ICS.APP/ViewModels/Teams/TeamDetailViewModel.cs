using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Xaml.Behaviors.Core;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class TeamDetailViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;

        public ICommand EditTeamMembersToggleVisibilityCommand { get; set; }
        public ICommand EditTeamInformationToggleVisibilityCommand { get; set; }
        public ICommand ShowTeamPostsToggleVisibilityCommand { get; }
        public ICommand EditTeamInformationCommand { get; }
        public ICommand CreateTeamCommand { get; set; }
        public ICommand DeleteTeamCommand { get; set; }


        public TeamModel Model { get; set; } = new TeamModel();
        public bool VisibilityEditing { get; set; } = false;
        public bool VisibilityCreating { get; set; } = false;

        private TeamLightModel ModelForMessages { get; set; }
        private UserLightModel LoggedUser { get; set; }
        public TeamDetailViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.facade = facade;
            this.messageBoxService = messageBoxService;
            this.mediator = mediator;

            EditTeamMembersToggleVisibilityCommand = new RelayCommand(EditTeamMembersToggleVisibility, LoggedUserIsAdminCheck);
            EditTeamInformationToggleVisibilityCommand =  new RelayCommand(EditTeamInformationToggleVisibility, LoggedUserIsAdminCheck);
            ShowTeamPostsToggleVisibilityCommand = new RelayCommand<TeamModel>(ShowTeamPostsToggleVisibility);

            CreateTeamCommand = new RelayCommand<TeamModel>(SaveNewTeam, CanSaveNewTeam);
            DeleteTeamCommand = new RelayCommand<TeamModel>(DeleteTeam, LoggedUserIsAdminCheck);
            EditTeamInformationCommand = new RelayCommand(UpdateTeamInformation, CanUpdateTeamInformation);

            mediator.Register<MyTeamSelectedMessage>(MyTeamSelected);
            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityEditingToFalse);
            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityCreatingToFalse);
            mediator.Register<UpdatedTeamSelectedMessage>(MyTeamUpdate);
            mediator.Register<EditTeamInformationToggleVisibilityMessage>(ToggleVisibilityEditingTeamInformation);
            mediator.Register<EditTeamMembersToggleVisibilityMessage>(TurnVisibilityEditingToFalse);
            mediator.Register<ShowTeamPostsToggleVisibilityMessage>(TurnVisibilityEditingToFalse);
            mediator.Register<CreateNewTeamMessage>(TurnVisibilityEditingToFalse);
            mediator.Register<CreateNewTeamMessage>(CreateNewTeam);
            mediator.Register<MyTeamsUpdated>(TurnVisibilityCreatingToFalse);
            mediator.Register<MyTeamsUpdated>(TurnVisibilityEditingToFalse);
            mediator.Register<UserAuthenticatedMessage>(LoadLoggedUser);
        }

        private void LoadLoggedUser(UserAuthenticatedMessage loggedUser)
        {
            this.LoggedUser = loggedUser.User;
        }

        private void DeleteTeam(TeamModel obj)
        {
            facade.Delete(this.Model);
            this.Model = new TeamModel();
            mediator.Send(new MyTeamsUpdated());
        }

        private bool LoggedUserIsAdminCheck(Object arg)
        {
            if (this.LoggedUser == null) 
                return false;
            if (this.Model.Admin == null)
                return false;
            return this.LoggedUser.Id == this.Model.Admin.Id;
        }

        private bool CanSaveNewTeam(object arg) =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Name)
            && !string.IsNullOrWhiteSpace(Model.Description);

        private void SaveNewTeam(object obj)
        {
            this.Save();
            mediator.Send(new MyTeamsUpdated());
        }

        private void CreateNewTeam(CreateNewTeamMessage adminToNewTeam)
        {
            this.VisibilityCreating = !VisibilityCreating;
            Model = new TeamModel{Admin = adminToNewTeam.NewAdmin, Name = "Write teams name here", Description = "Write description here"};
        }


        private bool CanUpdateTeamInformation(object obj) =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Name);

        private void UpdateTeamInformation(object obj)
        {
            this.Save();
            mediator.Send(new UpdatedTeamSelectedMessage{UpdatedTeam = ModelForMessages});
        }

        private void TurnVisibilityEditingToFalse(IMessage obj)
        {
            this.VisibilityEditing = false;
        }
        private void TurnVisibilityCreatingToFalse(IMessage obj)
        {
            this.VisibilityCreating = false;
        }

        private void EditTeamMembersToggleVisibility(object obj) => mediator.Send(new EditTeamMembersToggleVisibilityMessage{TeamLightModel = this.ModelForMessages});
        private void EditTeamInformationToggleVisibility(object obj) => mediator.Send(new EditTeamInformationToggleVisibilityMessage());
        private void ShowTeamPostsToggleVisibility(TeamModel model) => mediator.Send(new ShowTeamPostsToggleVisibilityMessage {TeamModel = model});
        private void MyTeamUpdate(UpdatedTeamSelectedMessage updatedTeam)
        {
            Load(updatedTeam.UpdatedTeam);
        }
        private void MyTeamSelected(MyTeamSelectedMessage myTeamSelectedMessage)
        {
            Load(myTeamSelectedMessage.MyTeam);
        }
        private void ToggleVisibilityEditingTeamInformation(EditTeamInformationToggleVisibilityMessage obj)
        {
            this.VisibilityEditing = !VisibilityEditing;
        }
        private void Save()
        {
            if (Model.Id == null)
            {
                Model = facade.Create(Model) as TeamModel;
                var lightTeam = new TeamLightModel{Id = Model.Id};
                var lightUser = new UserLightModel{Id = Model.Admin.Id};
                facade.JoinUserToTeam(lightUser,lightTeam);
            }
            else
            {
                facade.Update(Model);
            }
        }

        private void Load(TeamLightModel team)
        {
            this.ModelForMessages = team;
            this.Model = facade.GetDetail(team) as TeamModel;
        }
    }
}
