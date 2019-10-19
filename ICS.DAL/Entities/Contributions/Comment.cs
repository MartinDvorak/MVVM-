namespace TeamsManager.DAL.Entities.Contributions {
    public class Comment : Contribution {
        public Post ParentContribution { get; set; }
    }
}
