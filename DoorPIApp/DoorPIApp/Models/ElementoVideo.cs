using MediaManager;
using MediaManager.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoorPIApp.Models
{
    class ElementoVideo
    {
        public static async Task VerVideoUrl(string urlTransmision, MediaType tipoVideo = MediaType.Hls)
        {
            var item = await CrossMediaManager.Current.Extractor.CreateMediaItem(urlTransmision);
            item.MediaType = tipoVideo;
            await CrossMediaManager.Current.Play(item);
        }
        public static async Task VerVideoArchivo(string dirTransmision, MediaType tipoVideo = MediaType.Video)
        {
            var item = await CrossMediaManager.Current.Extractor.CreateMediaItemFromResource(dirTransmision);
            item.MediaType = tipoVideo;
            await CrossMediaManager.Current.Play(item);



            /*
             * When playing from a Resource you should add your media file for example to the Assets or raw folder on Android, and the Resources folder on iOS.
            For example:

                await CrossMediaManager.Current.PlayFromAssembly("somefile.mp3", typeof(BaseViewModel).Assembly);
                await CrossMediaManager.Current.PlayFromResource("assets:///somefile.mp3");
                await CrossMediaManager.Android.PlayFromResource(Resource.Raw.somefile.ToString());
             */
        }
    }
}
