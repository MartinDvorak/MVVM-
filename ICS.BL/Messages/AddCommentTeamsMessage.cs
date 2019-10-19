using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.ContributionModels;

namespace TeamsManager.BL.Messages
{
    public class AddCommentTeamsMessage : IMessage
    {
        public PostModel PostModel { get; set; }
    }
}
