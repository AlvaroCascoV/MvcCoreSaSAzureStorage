using MvcCoreSaSAzureStorage.Models;
using System.Xml.Linq;

namespace MvcCoreSaSAzureStorage.Helpers
{
    public class HelperXML
    {
        //con este helper leeremos el archivo xml incrustado en documents
        private XDocument document;
        public HelperXML()
        {
            string pathResourceXML = "MvcCoreSaSAzureStorage.Documents.alumnos_tables.xml";
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(pathResourceXML);
            this.document = XDocument.Load(stream);
        }

        public List<Alumno> GetAlumnos()
        {
            var consulta = from datos in this.document.Descendants("alumno")
                           select new Alumno
                           {
                               IdAlumno = int.Parse(datos.Element("idalumno").Value),
                               Curso = datos.Element("curso").Value,
                               Nombre = datos.Element("nombre").Value,
                               Apellidos = datos.Element("apellidos").Value,
                               Nota = int.Parse(datos.Element("nota").Value)
                           };
            return consulta.ToList();
        }

    }
}
