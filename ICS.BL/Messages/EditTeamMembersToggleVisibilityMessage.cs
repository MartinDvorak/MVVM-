
using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class EditTeamMembersToggleVisibilityMessage : IMessage
    {
        public TeamLightModel TeamLightModel;
    }
}
