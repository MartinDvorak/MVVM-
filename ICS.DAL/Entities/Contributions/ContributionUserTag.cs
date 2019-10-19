using System;
using System.Collections.Generic;
using System.Text;
using TeamsManager.DAL.Entities.BaseEntity;

namespace TeamsManager.DAL.Entities.Contributions {
    public class ContributionUserTag {
        public int? ContributionId { get; set; }
        public int? UserId { get; set; }
        public Contribution Contribution { get; set; }
        public User User { get; set; }
    }
}
