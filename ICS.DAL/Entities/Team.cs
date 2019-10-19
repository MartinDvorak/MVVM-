using System.Collections.Generic;
using TeamsManager.DAL.Entities.BaseEntity;
using TeamsManager.DAL.Entities.Contributions;

namespace TeamsManager.DAL.Entities
{
    public class Team : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public User Admin { get; set; }
        public ICollection<UserTeamMember> TeamMembers { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

}
