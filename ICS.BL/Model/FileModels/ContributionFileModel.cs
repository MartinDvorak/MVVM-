using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Model.FileModels
{
    public class ContributionFileModel : FileModel
    {
        public ContributionLightModel AssociatedContribution { get; set; }
        public SupportedFormatFile FileFormat { get; set; }
    }
}
