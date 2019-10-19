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
    public class TeamsMembersListViewModel :BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;
        public ICommand RemoveUserCommand { get; set; }
        public ICommand DelegateAdminPositionCommand { get; set; }
        public ObservableCollection<UserLightModel> TeamsMembers { get; set; } = new ObservableCollection<UserLightModel>();

        private TeamLightModel SelectedTeam {get; set; }

        public TeamsMembersListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            RemoveUserCommand = new RelayCommand(RemoveMember, CanRemoveMember);
            DelegateAdminPositionCommand = new RelayCommand(DelegateAdminPosition, CanDelegateAdminPosition);

            mediator.Register<MyTeamSelectedMessage>(MyTeamMembersLoad);
            mediator.Register<TeamMembersUpdatedMessage>(UpdateCollection);
        }

        private bool CanDelegateAdminPosition(object selectedAdmin)
        {
            return selectedAdmin != null;
        }
        public void DelegateAdminPosition(object newAdminUser)
        {
            facade.DelegateAdminPosition(newAdminUser as UserLightModel, this.SelectedTeam);
            mediator.Send(new MyTeamSelectedMessage{MyTeam = SelectedTeam});
        }
        private bool CanRemoveMember(object selectedMember)
        {
            return selectedMember != null && !facade.IsUserAdminInTeam(selectedMember as UserLightModel, this.SelectedTeam);
        }

        private void RemoveMember(object selectedMember)
        {
            DeleteUserFromTeam(selectedMember as UserLightModel);
        }

        private void UpdateCollection(TeamMembersUpdatedMessage teamMembersUpdatedMessage)
        {
            Load(teamMembersUpdatedMessage.UpdatedTeam);
        }

        private void MyTeamMembersLoad(MyTeamSelectedMessage myTeamSelectedMessage)
        {
            Load(myTeamSelectedMessage.MyTeam);
        }

        private void DeleteUserFromTeam(UserLightModel userLightModel)
        {
            facade.DeleteUserFromTeam(userLightModel, this.SelectedTeam);
            mediator.Send(new TeamMembersUpdatedMessage { UpdatedTeam = this.SelectedTeam });
        }

        private void Load(TeamLightModel selectedTeam)
        {
            this.SelectedTeam = selectedTeam;
            TeamsMembers.Clear();
            var teamMembers = facade.GetAllMembers(selectedTeam)
                .ToList();
            TeamsMembers.AddRange(teamMembers);
        }
    }
}
