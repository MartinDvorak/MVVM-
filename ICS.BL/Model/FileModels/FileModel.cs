using System;
using System.Collections.Generic;
using System.Text;

namespace TeamsManager.BL.Model.FileModels
{
    public class FileModel : BaseModel.Model
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
