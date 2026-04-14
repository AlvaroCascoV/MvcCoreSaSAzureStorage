using Azure;
using Azure.Data.Tables;

namespace MvcCoreSaSAzureStorage.Models
{
    public class Alumno : ITableEntity
    {
        private int _IdAlumno;
        public int IdAlumno
        {
            get { return _IdAlumno; }
            set
            {
                this._IdAlumno = value;
                RowKey = value.ToString();
            }
        }
        private string _Curso;
        public string Curso
        {
            get { return _Curso; }
            set
            {
                this._Curso = value;
                PartitionKey = value;
            }
        }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Nota { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
