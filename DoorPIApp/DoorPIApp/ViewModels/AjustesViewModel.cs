using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;

namespace DoorPIApp.ViewModels
{
    class AjustesViewModel : BaseViewModel
    {
        private string usuario;
        public string Usuario { get=>usuario; set=> SetProperty(ref usuario, value); }

        private string contrasena;
        public string Contrasena { get => contrasena; set => SetProperty(ref contrasena, value); }
    }
}
