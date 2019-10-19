using TeamsManager.DAL.Entities.Contributions;

namespace TeamsManager.DAL.Entities.Files {
    public class ContributionFile : File {
        public Contribution AssociatedContribution { get; set; }
        public SupportedFormatFile FileFormat { get; set; }
    }

}
