using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.DAL.Entities.BaseEntity;

namespace TeamsManager.DAL.Entities {
    public class UserTeamMember{
        public int? UserId { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public User User { get; set; }
    }
}
