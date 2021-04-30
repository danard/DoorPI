using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace DoorPIApp.ViewModels
{
    class AjustesViewModel : BaseViewModel
    {
        public AjustesViewModel()
        {
            GuardarPreferenciasCommand = new AsyncCommand(GuardarPreferencias);
            Usuario = Preferences.Get("Usuario", string.Empty);
            Contrasena = Preferences.Get("Contrasena", string.Empty);
        }
        private string usuario;
        public string Usuario { get => usuario; set => SetProperty(ref usuario, value); }

        private string contrasena;
        public string Contrasena { get => contrasena; set => SetProperty(ref contrasena, value); }

        public ICommand GuardarPreferenciasCommand { get; }
        private async Task GuardarPreferencias()
        {
            Preferences.Set("Usuario", Usuario);
            Preferences.Set("Contrasena", Contrasena);
        }
    }
}
