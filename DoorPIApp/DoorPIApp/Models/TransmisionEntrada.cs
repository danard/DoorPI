using System.Threading.Tasks;
using DoorPIApp.Services;
using Newtonsoft.Json;

namespace DoorPIApp.Models
{
    class TransmisionEntrada
    {
        class Identificador
        {
            public string usuario { get; set; }
            public string contrasena { get; set; }
        }
        static Identificador LeerIdAjustes()
        {
            //Falta implementar => Leer ajustes
            return new Identificador() { usuario = "sara", contrasena="1234" };
        }

        public static string JsonIdentificador()
        {
            Identificador id = LeerIdAjustes();
            return JsonConvert.SerializeObject(id);
        }

        public static async Task<string> UrlTransmision()
        {
            string jsonId = JsonIdentificador();
            return await Servidor.ObtenerEnlace(jsonId);
        }

        public static string UrlTransmisionPrueba()
        {
            return "https://austchannel-live.akamaized.net/hls/live/2002736/austchannel-sport/master.m3u8";
        }
    }
}
