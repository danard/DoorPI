﻿using DoorPIApp.Models;
using DoorPIApp.Services;
using MvvmHelpers;
using Xamarin.Forms;
using System.IO;
using System;
using Plugin.LocalNotification;

namespace DoorPIApp.ViewModels
{
    class UltimosAccesosViewModel : BaseViewModel
    {
        public UltimosAccesosViewModel()
        {
            AccesosHogar = AccesoHogar.AccesoHogarMock();
            MessagingCenter.Instance.Subscribe<App, byte[]>(this, Servidor.topicMqtt, UltimoAccesoRecibido);
        }
        public ObservableRangeCollection<AccesoHogar> AccesosHogar { get; }

        private void UltimoAccesoRecibido(App app, byte[] imagen)
        {
            var sourceStream = new MemoryStream(imagen);
            //DoorPIApp.Services.FileService.SavePicture(DateTime.Now.ToString(), sourceStream, "DoorPi");
            var source = ImageSource.FromStream(() => sourceStream);
            var acceso = new AccesoHogar(
                DateTime.Now,
                source,
                String.Empty);

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
    }
}
