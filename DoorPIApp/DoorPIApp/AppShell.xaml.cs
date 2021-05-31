using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoorPIApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Ajustes como pestaña inicial por defecto
            CurrentItem = AccesosTab;
        }
    }
}