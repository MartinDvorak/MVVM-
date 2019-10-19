using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Facade
{
    public interface ITeamsManagement
    {
        IEnumerable<TeamLightModel> FindMyTeams(UserLightModel user);
        IEnumerable<UserLightModel> GetAllMembers(TeamLightModel team);
        TeamLightModel ConvertTeamModelToTeamLightModel(TeamModel fullModel);
    }
}
