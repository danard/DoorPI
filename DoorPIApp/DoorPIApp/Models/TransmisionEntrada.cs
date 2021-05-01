using System.Threading.Tasks;
using DoorPIApp.Services;

namespace DoorPIApp.Models
{
    class TransmisionEntrada
    {
        public static async Task<string> UrlTransmision()
        {
            string jsonId = Servidor.JsonIdentificador();
            return await Servidor.ObtenerEnlace(jsonId);
        }

        public static string UrlTransmisionPrueba()
        {
            return "https://austchannel-live.akamaized.net/hls/live/2002736/austchannel-sport/master.m3u8";
        }
    }
}
