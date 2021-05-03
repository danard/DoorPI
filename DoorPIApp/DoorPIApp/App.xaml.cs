using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;
using System.Text;
using DoorPIApp.Services;
using Plugin.LocalNotification;
using System.IO;

namespace DoorPIApp
{
    public partial class App : Application
    {
        IManagedMqttClient mqttClient;
        public App()
        {
            InitializeComponent();

            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;
            MainPage = new AppShell();

            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                Task.Run(async () =>
                {

                });
                return true;
            });
        }

        private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            Task.Run(async () => await Shell.Current.GoToAsync("//tabs/accesos"));
        }

        void IniciarMqtt()
        {
            //Setup and start a managed MQTT client.
            var options = new ManagedMqttClientOptionsBuilder()
                              .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                              .WithClientOptions(new MqttClientOptionsBuilder()
                              .WithClientId(Servidor.ClientID)
                              .WithTcpServer("nattech.fib.upc.edu", 40331)
                              .Build())
                          .Build();
            mqttClient = new MqttFactory().CreateManagedMqttClient();
            Task.Run(async () =>
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(Servidor.topicMqtt).Build());
                await mqttClient.StartAsync(options);
            });

            mqttClient.UseApplicationMessageReceivedHandler(m =>
            {
                MqttMessageHandler(ref m);
            });
        }

        void MqttMessageHandler(ref MqttApplicationMessageReceivedEventArgs m)
        {
            var payload = m.ApplicationMessage.Payload;
            if (m.ApplicationMessage.Topic == Servidor.topicMqtt)
            {
                MessagingCenter.Instance.Send(this, Servidor.topicMqtt, payload);
            }
        }
        void PararMqtt()
        {
            Task.Run(async () => await mqttClient?.StopAsync());
        }

        protected override void OnStart()
        {
            IniciarMqtt();
        }

        protected override void OnSleep()
        {
            //PararMqtt();
        }

        protected override void OnResume()
        {
            IniciarMqtt();
        }
    }
}
