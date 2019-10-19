
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Model.LightModel.FileLightModels
{
    public class ProfileImageLightModel : BaseModel.Model
    {
        public SupportedFormatPicture PictureFormat { get; set; }
        public byte[] Content { get; set; }
    }
}
