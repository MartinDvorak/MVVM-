using TeamsManager.BL.Model.LightModel;
using System.Drawing;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Facade
{
    public interface IUserManagement
    {
        UserLightModel FindUserByEmail(string email);
        string HashPassword(string password);
        bool IsEmailValid(string email);
        bool AreUserDataValid(string firstName, string lastName, string email, string passwordHash,
            string passwordConfirmationHash);
        byte[] ConvertImageToByteArray(Image image);
        SupportedFormatPicture ConvertFileExtenstionToEnum(string dlgDefaultExt);
    }
}
