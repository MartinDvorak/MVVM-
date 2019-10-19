using TeamsManager.BL.Model.LightModel;

namespace TeamsManager.BL.Messages
{
    public class SuccessfullyRegisteredMessage : IMessage
    {
        public UserLightModel User { get; set; }
    }
}
