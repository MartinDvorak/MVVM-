using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Messages
{
    public class TaggedInCommentSelectedMessage : IMessage
    {
        public CommentLightModel Comment { get; set; }
    }
}
