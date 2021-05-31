using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using MediaManager;
using Plugin.LocalNotification;
using Android.Content;
using Android.Graphics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace DoorPIApp.Droid
{
    [Activity(Label = "DoorPIApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Pop up plugin
            Rg.Plugins.Popup.Popup.Init(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossMediaManager.Current.Init(this);

            NotificationCenter.CreateNotificationChannel();
            LoadApplication(new App());
            NotificationCenter.NotifyNotificationTapped(Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
        public void SavePictureToDisk(Bitmap source, string imageName)
        {
            try
            {
                Task.Run(() =>
                {
                    var documentsDirectry = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);
                    string pngFilename = System.IO.Path.Combine(documentsDirectry.AbsolutePath, imageName);

                    //If the image already exists, delete, and make way for the updated one
                    if (File.Exists(pngFilename))
                    {
                        File.Delete(pngFilename);
                    }

                    using (FileStream fs = new FileStream(pngFilename, FileMode.OpenOrCreate))
                    {
                        source.Compress(Bitmap.CompressFormat.Jpeg, 50, fs);
                        fs.Close();
                    }

                });
            }
            catch (Exception e)
            {

            }
        }
    }
}