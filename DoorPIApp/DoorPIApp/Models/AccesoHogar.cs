using MediaManager.Library;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace DoorPIApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    class AccesoHogar
    {
        public AccesoHogar(string fecha, string imagenMiniatura, string dirVideo)
        {
            FechaAcceso = fecha;
            MiniaturaDir = imagenMiniatura;
            VideoDirectorio = dirVideo;

            //Commands
            VerVideoCommand = new AsyncCommand(VerVideoAsyncTask);
            EliminarVideoCommand = new AsyncCommand(EliminarVideo);
        }
        //public AccesoHogar(DateTime fecha, ImageSource imagenMiniatura, string dirVideo)
        //{
        //    FechaAcceso = fecha;
        //    MiniaturaSource = imagenMiniatura;
        //    VideoDirectorio = dirVideo;

        //    //Commands
        //    VerVideoCommand = new AsyncCommand(VerVideoAsyncTask);
        //    EliminarVideoCommand = new AsyncCommand(EliminarVideo);
        //}


        public string Titulo
        {
            get
            {
                return FechaAcceso;
            }
        }
        [JsonProperty]
        public string FechaAcceso { get; set; }
        [JsonProperty]
        public string MiniaturaDir { get; set; }
        //public ImageSource MiniaturaSource { get; set; }
        [JsonProperty]
        public string VideoDirectorio { get; set; }

        public AsyncCommand VerVideoCommand { get; }
        public async Task VerVideoAsyncTask()
        {
            await ElementoVideo.VerVideoArchivo(VideoDirectorio);
        }

        public AsyncCommand EliminarVideoCommand { get; }

        private async Task EliminarVideo()
        {
            if(File.Exists(MiniaturaDir))
            {
                File.Delete(MiniaturaDir);
            }
            MessagingCenter.Instance.Send(this, "EliminarAcceso");
        }
        //public static ObservableRangeCollection<AccesoHogar> AccesoHogarMock()
        //{
        //    var acceso1 = new AccesoHogar(DateTime.Now, "door1.png", "");
        //    var acceso2 = new AccesoHogar(DateTime.Now.AddDays(-1), "door2.png", "");
        //    var acceso3 = new AccesoHogar(DateTime.Now.AddDays(-3), "door3.png", "");
        //    var acceso4 = new AccesoHogar(DateTime.Now.AddDays(-4), "door1.png", "");
        //    var acceso5 = new AccesoHogar(DateTime.Now.AddDays(-5), "door2.png", "");

        //    var acceso6 = new AccesoHogar(DateTime.Now.AddDays(-5), "/storage/emulated/0/Android/data/com.companyname.doorpiapp/files/image.jpg", "");
        //    return new ObservableRangeCollection<AccesoHogar>
        //    {
        //        acceso1,
        //        acceso2,
        //        acceso3,
        //        acceso4,
        //        acceso5,
        //        acceso6
        //    };
        //}
    }
}
