using MediaManager;
using MediaManager.Library;
using MvvmHelpers;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
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
            await ElementoVideo.VerVideoUrl(UrlTransmision);
        }
    }
}
