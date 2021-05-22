using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net.Mqtt;

namespace DoorPIApp.Services
{
    class Servidor
    {
        class Identificador
        {
            public string usuario { get; set; }
            public string contrasena { get; set; }
            public Identificador()
            {
                usuario = Preferences.Get("Usuario", string.Empty);
                contrasena = Preferences.Get("Contrasena", string.Empty);
            }
        }
        private static readonly HttpClient client = new HttpClient();
        
        public const string ServidorEnlace = "http://nattech.fib.upc.edu:40330/stream";
        public const string ServidorImagen = "http://nattech.fib.upc.edu:40330/img";

        //MQTT
        public static string ServidorMqtt 
        {
            get { return "nattech.fib.upc.edu"; }
        }
        public static int PuertoMqtt
        {
            get { return 40331; }
        }
        public static string topicMqtt
        {
            get
            {
                var id = new Identificador();
                return "access-images-" + id.usuario + id.contrasena;
            }
        }
        public static string ClientID
        {
            get
            {
                return "Usuario";
            }
        }

        public static string JsonIdentificador()
        {
            Identificador id = new Identificador();
            return JsonConvert.SerializeObject(id);
        }

        public static async Task<string> ObtenerEnlace(string jsonIDserializado, string url = ServidorEnlace)
        {
            var json = new StringContent(jsonIDserializado, Encoding.UTF8, "application/json");
            try
            {
                var enlace = await client.PostAsync(url, json);
                if (enlace.IsSuccessStatusCode)
                {
                    return await enlace.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static async Task<byte[]> ObtenerImagen(string jsonIDserializado, string url = ServidorImagen)
        {
            var json = new StringContent(jsonIDserializado, Encoding.UTF8, "application/json");
            try
            {
                var enlace = await client.PostAsync(url, json);
                if (enlace.IsSuccessStatusCode)
                {
                    var content = enlace.Content;
                    return await content.ReadAsByteArrayAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }


}
