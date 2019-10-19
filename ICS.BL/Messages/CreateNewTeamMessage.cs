using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class CreateNewTeamMessage:IMessage
    {
        public UserLightModel NewAdmin;
    }
}
