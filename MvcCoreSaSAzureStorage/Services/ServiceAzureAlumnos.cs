using Azure.Data.Tables;
using MvcCoreSaSAzureStorage.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MvcCoreSaSAzureStorage.Services
{
    public class ServiceAzureAlumnos
    {
        private TableClient tableAlumnos;

        //NECESITAMOS LA URL DE ACCESO AL TOKEN (MINIMAL API)
        private string UrlApi;
        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiAzureToken");
        }

        private async Task<string> GetTokenAsync(string curso) //nuestro token está filtrado por curso
        {
            using (HttpClient client = new HttpClient())
            {
                string request = $"token/" + curso;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("token").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Alumno>> GetAlumnosAsync(string curso)
        {
            string token = await this.GetTokenAsync(curso);
            //CONVERTIMOS A URI EL STRING
            Uri uriToken = new Uri(token);
            //SIMPLEMENTE ACCEDEMOS AL RECURSO (TABLA AZURE STORAGE)
            //MEDIANTE EL TOKEN
            this.tableAlumnos = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();
            var query = this.tableAlumnos.QueryAsync<Alumno>(filter: ""); //filter vacío para traer todos los registros
            await foreach (Alumno alumno in query)
            {
                alumnos.Add(alumno);
            }
            return alumnos;
        }
    }
}
