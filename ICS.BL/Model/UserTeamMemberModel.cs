using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Model
{
    public class UserTeamMemberModel
    {
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public UserLightModel User { get; set; }
        public TeamLightModel Team { get; set; }
    }
}