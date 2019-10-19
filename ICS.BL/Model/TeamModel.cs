using System.Collections.Generic;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Model
{
    public class TeamModel : BaseModel.Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public UserLightModel Admin { get; set; }
        public  ICollection<UserTeamMemberModel> Members { get; set; }
        public ICollection<PostLightModel> Posts { get; set; }
    }
}