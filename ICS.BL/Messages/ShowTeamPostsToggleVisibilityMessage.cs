using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model;

namespace TeamsManager.BL.Messages
{
    public class ShowTeamPostsToggleVisibilityMessage :IMessage
    {
        public TeamModel TeamModel;
    }
}
