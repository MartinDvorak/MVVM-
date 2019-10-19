using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Messages
{
    public class TaggedInPostSelectedMessage : IMessage
    {
        public PostLightModel Post { get; set; }
    }
}
