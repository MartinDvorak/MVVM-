using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsManager.BL.Model.LightModel.ContributionLightModels
{
    public abstract class ContributionLightModel : BaseModel.Model
    {
        public UserLightModel Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
