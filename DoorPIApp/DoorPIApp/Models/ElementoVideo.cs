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
            try
            {
                var item = await CrossMediaManager.Current.Extractor.CreateMediaItem(urlTransmision);
                item.MediaType = tipoVideo;
                await CrossMediaManager.Current.Play(item);
            }
            catch
            {
                throw new ArgumentException("Error abriendo el Reproductor con el enlace proporcionado.");
            }
        }
        public static async Task VerVideoArchivo(string dirTransmision, MediaType tipoVideo = MediaType.Video)
        {
            var item = await CrossMediaManager.Current.Extractor.CreateMediaItemFromResource(dirTransmision);
            item.MediaType = tipoVideo;
            await CrossMediaManager.Current.Play(item);
        }
        public static async Task PararVideo()
        {
            await CrossMediaManager.Current.Stop();
        }
    }
}
