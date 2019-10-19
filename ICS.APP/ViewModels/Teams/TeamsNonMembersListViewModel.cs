using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Extensions;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class TeamsNonMembersListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;

        public ObservableCollection<UserLightModel> TeamsNonMembers { get; set; } =
            new ObservableCollection<UserLightModel>();

        public bool Visibility { get; set; } = false;
        private TeamLightModel TeamWhereArentUsers { get; set; }

        public TeamsNonMembersListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            AddUserCommand = new RelayCommand(SaveNonMember, CanSaveNonMember);

            mediator.Register<EditTeamMembersToggleVisibilityMessage>(EditTeamMembersToggleVisibility);
            mediator.Register<TeamMembersUpdatedMessage>(UpdateCollection);
            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamInformationToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<ShowTeamPostsToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<CreateNewTeamMessage>(TurnVisibilityToFalse);
            mediator.Register<MyTeamsUpdated>(TurnVisibilityToFalse);
        }

        private void TurnVisibilityToFalse(IMessage obj)
        {
            this.Visibility = false;
        }

        private void UpdateCollection(TeamMembersUpdatedMessage teamMembersUpdatedMessage)
        {
            this.Load(teamMembersUpdatedMessage.UpdatedTeam);
        }

        public bool CanSaveNonMember(object nonMemberSelectedModel)
        {
            return nonMemberSelectedModel != null;
        }

        public void SaveNonMember(object nonMemberModel)
        {
            facade.JoinUserToTeam(nonMemberModel as UserLightModel, this.TeamWhereArentUsers);
            mediator.Send(new TeamMembersUpdatedMessage{UpdatedTeam = TeamWhereArentUsers});
        }

        public ICommand AddUserCommand { get; set; }

        private void EditTeamMembersToggleVisibility(EditTeamMembersToggleVisibilityMessage editTeamMembersToggleVisibilityMessage)
        {
            Visibility = !Visibility;
            if(Visibility)
                Load(editTeamMembersToggleVisibilityMessage.TeamLightModel);
        }

        public void Load(TeamLightModel team)
        {
            this.TeamWhereArentUsers = new TeamLightModel{Id = team.Id,Name = team.Name};
            TeamsNonMembers.Clear();
            var teamNonMembers = facade.GetAllNonMembers(team)
                .ToList();
            TeamsNonMembers.AddRange(teamNonMembers);
        }
    }
}

