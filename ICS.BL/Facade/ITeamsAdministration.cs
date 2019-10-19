using System.Collections.Generic;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Facade
{
    public interface ITeamsAdministration
    {
        void JoinUserToTeam(UserLightModel user, TeamLightModel team);
        void DeleteUserFromTeam(UserLightModel user, TeamLightModel team);
        IEnumerable<UserLightModel> GetAllNonMembers(TeamLightModel team);
        void DelegateAdminPosition(UserLightModel user, TeamLightModel team);

        bool IsUserAdminInTeam(UserLightModel user, TeamLightModel team);
    }
}
