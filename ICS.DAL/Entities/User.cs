using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TeamsManager.DAL.Entities.BaseEntity;
using TeamsManager.DAL.Entities.Contributions;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.DAL.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; } //hash
        public string UserDescription { get; set; }

        public ProfileImage Photo { get; set; }
        public ICollection<Contribution> MyContributions { get; set; }
        public ICollection<UserTeamMember> UserTeams { get; set; }
        public ICollection<Team> AdministratedTeams { get; set; }
        public ICollection<ContributionUserTag> ContributionUserTags { get; set; }
    }
}
