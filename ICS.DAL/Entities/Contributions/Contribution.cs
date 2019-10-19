using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamsManager.DAL.Entities.BaseEntity;
using TeamsManager.DAL.Entities.Files;
using TeamsManager.DAL.Entities;

namespace TeamsManager.DAL.Entities.Contributions {
    public abstract class Contribution : Entity {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public User Author { get; set; }
        public ICollection<ContributionFile> AssociatedFiles { get; set; }
        public ICollection<ContributionUserTag> ContributionUserTags { get; set; }
    }
}
