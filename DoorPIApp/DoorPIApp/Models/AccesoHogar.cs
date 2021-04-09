using MediaManager.Library;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoorPIApp.Models
{
    class AccesoHogar
    {
        public AccesoHogar(DateTime fecha, string imagenMiniatura, string dirVideo)
        {
            FechaAcceso = fecha;
            Miniatura = imagenMiniatura;
            VideoDirectorio = dirVideo;

            //Commands
            VerVideoCommand = new AsyncCommand(VerVideoAsyncTask);
            EliminarVideoCommand = new AsyncCommand(EliminarVideo);
        }

        public string Titulo
        {
            get
            {
                return FechaAcceso.ToLongDateString();
            }
        }
        public DateTime FechaAcceso { get; set; }
        public string Miniatura { get; set; }
        public string VideoDirectorio { get; set; }

        public AsyncCommand VerVideoCommand { get; }
        public async Task VerVideoAsyncTask()
        {
            await ElementoVideo.VerVideoArchivo(VideoDirectorio);
        }

        public AsyncCommand EliminarVideoCommand { get; }

        private async Task EliminarVideo()
        {
            //Find way to delete video from phone
        }
        public static ObservableRangeCollection<AccesoHogar> AccesoHogarMock()
        {
            var acceso1 = new AccesoHogar(DateTime.Now, "door1.png", "");
            var acceso2 = new AccesoHogar(DateTime.Now.AddDays(-1), "door2.png", "");
            var acceso3 = new AccesoHogar(DateTime.Now.AddDays(-3), "door3.png", "");
            var acceso4 = new AccesoHogar(DateTime.Now.AddDays(-4), "door1.png", "");
            var acceso5 = new AccesoHogar(DateTime.Now.AddDays(-5), "door2.png", "");
            var acceso6 = new AccesoHogar(DateTime.Now.AddDays(-5), "door3.png", "");

            return new ObservableRangeCollection<AccesoHogar>
            {
                acceso1,
                acceso2,
                acceso3,
                acceso4,
                acceso5,
                acceso6
            };
        }
    }
}
