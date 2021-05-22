using System;
using System.Threading.Tasks;
using DoorPIApp.Services;

namespace DoorPIApp.Models
{
    class TransmisionEntrada
    {
        public static async Task<string> UrlTransmision()
        {
            string jsonId = Servidor.JsonIdentificador();
            string enlace = await Servidor.ObtenerEnlace(jsonId);
            if (enlace == string.Empty)
                throw new ArgumentException("No se ha podido obtener el enlace con las credenciales proporcionadas.");
            return enlace;
        }

        public static string UrlTransmisionPrueba()
        {
            return "https://austchannel-live.akamaized.net/hls/live/2002736/austchannel-sport/master.m3u8";
        }
    }
}
