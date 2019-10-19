using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Model.ContributionModels
{
    public class ContributionUserTagModel
    {
        public int? ContributionId { get; set; }
        public int? UserId { get; set; }
        public ContributionLightModel Contribution { get; set; }
        public UserLightModel User { get; set; }
    }
}