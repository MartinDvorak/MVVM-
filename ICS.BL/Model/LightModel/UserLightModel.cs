using System;
using System.Collections.Generic;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Model.LightModel
{
    public class UserLightModel: BaseModel.Model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserDescription { get; set; }
        public ProfileImageLightModel ProfilePicture { get; set; } 
    }

}


//todo erase seed text