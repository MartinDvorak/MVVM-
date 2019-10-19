using TeamsManager.DAL.Entities.BaseEntity;

namespace TeamsManager.DAL.Entities.Files {
    public abstract class File : Entity {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
