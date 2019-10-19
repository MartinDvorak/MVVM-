using System.Collections.Generic;

namespace TeamsManager.DAL.Entities.Contributions {
    public class Post : Contribution {
        public string Title { get; set; }
        public Team CorrespondingTeam { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
