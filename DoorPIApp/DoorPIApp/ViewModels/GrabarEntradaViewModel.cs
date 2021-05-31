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
            VerTransmisionCommand = new AsyncCommand(VerTransmision);
            PararTransmisionCommand = new AsyncCommand(PararTransmision);
        }

        private string urlTransmision;
        public string UrlTransmision { get => urlTransmision; set => SetProperty(ref urlTransmision, value); }

        private const string mensajeErrorURL = "No se puede reproducir la transmisión.\nComprueba que los ajustes son correctos.";

        private string mensajeError;
        public string MensajeError { get => mensajeError; set => SetProperty(ref mensajeError, value); }

        public ICommand VerTransmisionCommand { get; }
        private async Task VerTransmision()
        {
            try
            {
                UrlTransmision = await TransmisionEntrada.UrlTransmision();
                await ElementoVideo.VerVideoUrl(UrlTransmision);
                MensajeError = String.Empty;
            }
            catch (ArgumentException)
            {
                mensajeError = mensajeErrorURL;
            }
        }

        public ICommand PararTransmisionCommand { get; }
        private async Task PararTransmision()
        {
            await ElementoVideo.PararVideo();
        }
    }
}
