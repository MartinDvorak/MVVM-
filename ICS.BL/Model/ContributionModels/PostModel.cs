 using System.Collections.Generic;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Model.ContributionModels
{
    public class PostModel : ContributionModel
    {
        public string Title { get; set; }
        public TeamLightModel CorrespondingTeam { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
    }
}
