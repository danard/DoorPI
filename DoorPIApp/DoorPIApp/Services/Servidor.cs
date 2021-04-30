using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace DoorPIApp.Services
{
    class Servidor
    {
        private static readonly HttpClient client = new HttpClient();
        public const string ServidorEnlace = "http://nattech.fib.upc.edu:40330/stream";

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
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }


}
