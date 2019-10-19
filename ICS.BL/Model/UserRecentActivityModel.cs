using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Model
{
    public class MyRecentActivityModel
    {
        public DateTime PublicationTime { get; set; }
        public TeamLightModel Team { get; set; }
        public ContributionLightModel Contribution { get; set; }
    }
}
