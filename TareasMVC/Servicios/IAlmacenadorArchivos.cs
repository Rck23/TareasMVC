using TareasMVC.Models;

namespace TareasMVC.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task Borrar(string ruta, string contenedor);

        // IFormFile <---- representa un archivo cualquiera en .net
        Task<AlmacenarArchivoResultado[]> Almacenar(string contenedor,
            IEnumerable<IFormFile> archivos);
    }
}
