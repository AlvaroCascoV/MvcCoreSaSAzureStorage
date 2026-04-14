using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSaSAzureStorage.Helpers;
using MvcCoreSaSAzureStorage.Models;

namespace MvcCoreSaSAzureStorage.Controllers
{
    public class MigracionController : Controller
    {
        //este controlador deberia estar en otro proyecto,
        //coge los datos del xml y los inserta en la tabla de azure storage
        private HelperXML helper;
        private IConfiguration configuration;
        public MigracionController(HelperXML helper, IConfiguration configuration)
        {
            this.helper = new HelperXML();
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string accion)
        {
            //EN ESTE METODO LO QUE NECESITAMOS SON LAS KEYS DE AZURE STORAGE.
            //ESTA FUNCIONALIDAD DEBERIA ESTAR EN OTRO PROYECTO, PERO PARA SIMPLIFICAR LO HEMOS PUESTO AQUI
            string azureKeys = this.configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient tableServiceClient = new TableServiceClient(azureKeys);
            TableClient tableClient = tableServiceClient.GetTableClient("alumnos");
            await tableClient.CreateIfNotExistsAsync();
            List<Alumno> alumnos = this.helper.GetAlumnos();
            foreach (Alumno alumno in alumnos)
            {
                await tableClient.AddEntityAsync<Alumno>(alumno);
            }
            ViewData["MENSAJE"] = "Migracion realizada correctamente";
            return View();
        }
    }
}
