using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoorPIApp.Services
{
    public interface IFileService
    {
        void SavePictureToDisk(Bitmap source, string imageName);
    }
}
