using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Model.ContributionModels
{
    public class CommentModel : ContributionModel
    {
        public PostLightModel ParentContribution { get; set; }
    }
}
