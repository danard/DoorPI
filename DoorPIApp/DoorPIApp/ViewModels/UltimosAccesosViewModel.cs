using DoorPIApp.Models;
using DoorPIApp.Services;
using MvvmHelpers;
using Xamarin.Forms;
using System.IO;
using System;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace DoorPIApp.ViewModels
{
    class UltimosAccesosViewModel : BaseViewModel
    {
        public UltimosAccesosViewModel()
        {
            LeerAccesosPreferencias();
            MessagingCenter.Instance.Subscribe<App, byte[]>(this, Servidor.topicMqtt, UltimoAccesoRecibido);
            MessagingCenter.Instance.Subscribe<App>(this, "GuardarAccesosPreferencias", GuardarAccesosPreferencias);
            MessagingCenter.Instance.Subscribe<AccesoHogar>(this, "EliminarAcceso", EliminarAccesoHogar);
        }

        private void EliminarAccesoHogar(AccesoHogar obj)
        {
            AccesosHogar.Remove(obj);
        }

        public ObservableRangeCollection<AccesoHogar> AccesosHogar { get; set; }
        private string accesosHogarKey = "AccesosHogar";
        private async void UltimoAccesoRecibido(App app, byte[] imagen)
        {
            var base64 = Convert.ToBase64String(imagen);
            var bytes = Convert.FromBase64String(base64);
            //MemoryStream str = new MemoryStream(bytes);
            //var source = ImageSource.FromStream(() => str);

            var filePath = Path.Combine(FileSystem.AppDataDirectory, DateTime.Now.ToString("ddhhmmssfffffff") + ".jpg");

            using (FileStream SourceStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                await SourceStream.WriteAsync(bytes, 0, bytes.Length);
            }

            var acceso = new AccesoHogar
                (
                    DateTime.Now.ToString("hh:mm, dddd MMMM yyyy"),
                    filePath,
                    String.Empty   //Falta implementar video
                );

            AccesosHogar.Insert(0, acceso);
            CrearNotificacionLocal();
        }

        void CrearNotificacionLocal()
        {
            var notificacion = new NotificationRequest
            {
                Title = "Nuevo acceso a tu hogar",
                Description = "Ver quién ha entrado"
            };
            NotificationCenter.Current.Show(notificacion);
        }

        private void LeerAccesosPreferencias()
        {
            string listSerializada = Preferences.Get(accesosHogarKey, string.Empty);
            if (listSerializada == string.Empty)
                AccesosHogar = new ObservableRangeCollection<AccesoHogar>();
            else
                AccesosHogar = JsonConvert.DeserializeObject<ObservableRangeCollection<AccesoHogar>>(listSerializada);
        }
        private void GuardarAccesosPreferencias(App app)
        {
            string accesosJson = JsonConvert.SerializeObject(AccesosHogar, Formatting.Indented);
            Preferences.Set(accesosHogarKey, accesosJson);
        }
    }
}
