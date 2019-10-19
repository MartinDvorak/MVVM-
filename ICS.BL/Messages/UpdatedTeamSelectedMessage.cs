using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class UpdatedTeamSelectedMessage : IMessage
    {
        public TeamLightModel UpdatedTeam;
    }
}
