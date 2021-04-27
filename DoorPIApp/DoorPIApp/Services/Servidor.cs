using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            var enlace = await client.PostAsync(url, json);
            if (enlace.IsSuccessStatusCode ) {
                return await enlace.Content.ReadAsStringAsync();
            } else
            {
                return "error";
            }
            
        }


        //public HttpResponse Get(string Url = ServidorEnlace)
        //{
        //    var request = WebRequest.Create(Url);
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    using (HttpWebResponse httpResponse = request.GetResponse() as HttpWebResponse)
        //    {
        //        return BuildResponse(httpResponse);
        //    }
        //}

        //private static HttpResponse BuildResponse(HttpWebResponse httpResponse)
        //{
        //    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var content = reader.ReadToEnd();
        //        var response = new HttpResponse();
        //        response.Content = content;
        //        response.HttpStatusCode = httpResponse.StatusCode;
        //        return response;
        //    }
        //}

        //public async Task<HttpResponse> GetAsync(string Url = ServidorEnlace)
        //{
        //    var request = WebRequest.Create(Url);
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    using (HttpWebResponse httpResponse = await request.GetResponseAsync() as HttpWebResponse)
        //    {
        //        return BuildResponse(httpResponse);
        //    }
    }


}
