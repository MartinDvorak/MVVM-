using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsManager.BL.Model.LightModel.ContributionLightModels
{
    public class PostLightModel : ContributionLightModel
    {
        public string Title { get; set; }
        public ICollection<CommentLightModel> Comments { get; set; }
    }
}
