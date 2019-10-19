using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Extensions;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class MyTeamsListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IMediator mediator;
        private readonly IBusinessLogicFacade facade;

        private UserLightModel logerUser { get; set; }

    public MyTeamsListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            MyTeamSelectedCommand = new RelayCommand<TeamLightModel>(MyTeamSelected);
            CreateNewTeamCommand = new RelayCommand(CreateNewTeam);

            mediator.Register<MyTeamsUpdated>(ReloadTeams);
            mediator.Register<UserAuthenticatedMessage>(LoadLoggedUser);

        }

    private void LoadLoggedUser(UserAuthenticatedMessage loggedUser)
    {
        this.logerUser = loggedUser.User;
        this.LoadMyTeams();
    }

    private void ReloadTeams(MyTeamsUpdated obj)
    {
        this.LoadMyTeams();
    }

    public ObservableCollection<TeamLightModel> MyTeams { get; set; } = new ObservableCollection<TeamLightModel>();
        public ICommand MyTeamSelectedCommand { get; }
        public ICommand CreateNewTeamCommand { get; }
        private void MyTeamSelected(TeamLightModel myTeamLightModel) => mediator.Send(new MyTeamSelectedMessage { MyTeam = myTeamLightModel });
        private void CreateNewTeam() => mediator.Send(new CreateNewTeamMessage { NewAdmin = this.logerUser });


        public void LoadMyTeams()
        {
            MyTeams.Clear();
            var myTeams = facade.FindMyTeams(this.logerUser)
                .ToList();
            MyTeams.AddRange(myTeams);
        }
    }
}
