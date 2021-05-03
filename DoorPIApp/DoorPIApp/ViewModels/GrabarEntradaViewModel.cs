using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using DoorPIApp.Models;
using System;

namespace DoorPIApp.ViewModels
{
    class GrabarEntradaViewModel : BaseViewModel
    {
        public GrabarEntradaViewModel()
        {
            //ObservableRangeCollection  (MVVM helpers)
            VerTransmisionCommand = new AsyncCommand(VerTransmision);
        }

        private string urlTransmision;
        public string UrlTransmision { get => urlTransmision; set => SetProperty(ref urlTransmision, value); }

        private string mensajeError;
        public string MensajeError { get => mensajeError; set => SetProperty(ref mensajeError, value); }

        public ICommand VerTransmisionCommand { get; }
        private async Task VerTransmision()
        {
            UrlTransmision = await TransmisionEntrada.UrlTransmision();
            if (UrlTransmision == string.Empty)
                MensajeError = "No se puede reproducir la transmisión.\nComprueba que los ajustes son correctos.";
            else
            {
                await ElementoVideo.VerVideoUrl(UrlTransmision);
                MensajeError = String.Empty;
            }
        }
    }
}
