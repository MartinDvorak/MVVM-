using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Model.FileModels
{
    public class ProfileImageModel : FileModel
    {
        public SupportedFormatPicture PictureFormat { get; set; }
    }
}