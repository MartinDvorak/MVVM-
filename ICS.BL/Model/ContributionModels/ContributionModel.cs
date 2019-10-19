using System;
using System.Collections.Generic;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.FileLightModels;

namespace TeamsManager.BL.Model.ContributionModels
{
    public abstract class ContributionModel : BaseModel.Model
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public UserLightModel Author { get; set; }
        public ICollection<ContributionFileLightModel> AssociatedFiles { get; set; }
        public ICollection<ContributionUserTagModel> ContributionUserTags { get; set; }
    }
}
