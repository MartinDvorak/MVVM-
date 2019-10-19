using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class MyTeamSelectedMessage : IMessage
    {
        public TeamLightModel MyTeam { get; set; }
    }
}