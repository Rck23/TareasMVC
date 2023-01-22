using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Entidades
{
    public class Tarea
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        // PROPIEDAD DE NAVEGACIÓN (se configura automaticamente como llave foranea)
        // Relación 1 a muchos
        public List<Paso> Pasos { get; set; }

        public List<ArchivoAdjunto> ArchivosAdjuntos { get; set; }


        //LLAVE FORANEA PARA MOSTRAR TAREAS POR USUARIO
        public string UsuarioCreacionId { get; set; }

        public IdentityUser UsuarioCreacion { get; set; }
    }
}
