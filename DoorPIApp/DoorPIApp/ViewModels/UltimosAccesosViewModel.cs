using DoorPIApp.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoorPIApp.ViewModels
{
    class UltimosAccesosViewModel : BaseViewModel
    {
        public UltimosAccesosViewModel()
        {
            AccesosHogar = AccesoHogar.AccesoHogarMock();
        }
        public ObservableRangeCollection<AccesoHogar> AccesosHogar { get; }

    }
}
