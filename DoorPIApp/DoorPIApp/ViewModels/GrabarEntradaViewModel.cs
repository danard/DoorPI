using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using DoorPIApp.Models;

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

        public ICommand VerTransmisionCommand { get; }
        private async Task VerTransmision()
        {
            UrlTransmision = await TransmisionEntrada.UrlTransmision();
            await ElementoVideo.VerVideoUrl(UrlTransmision);
        }
    }
}
