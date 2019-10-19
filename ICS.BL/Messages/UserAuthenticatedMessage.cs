using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class UserAuthenticatedMessage : IMessage
    {
        public UserLightModel User { get; set; }
    }
}
