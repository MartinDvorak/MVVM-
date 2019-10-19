using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class TeamMembersUpdatedMessage : IMessage
    {
        public TeamLightModel UpdatedTeam;
    }
}
