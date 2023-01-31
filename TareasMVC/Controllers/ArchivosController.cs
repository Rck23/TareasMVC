using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly string contenedor = "archivosadjuntos";

        public ArchivosController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos, 
            IServicioUsuarios servicioUsuarios)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
            _servicioUsuarios = servicioUsuarios;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<IEnumerable<ArchivoAdjunto>>> Post(int tareaId,
            [FromForm] IEnumerable<IFormFile> archivos) // FromForm <---- permite transmitir un formato que no nomas sea JSON o campos básicos. 
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId); 

            if (tarea == null)
            {
                return NotFound();
            }

            if(tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            var existenArchivosAdjuntos = await _context.ArchivosAdjuntos
                    .AnyAsync(a => a.TareaId == tareaId);

            var ordenMayor = 0; 
            if (existenArchivosAdjuntos)
            {
                ordenMayor = await _context.ArchivosAdjuntos
                    .Where(a => a.TareaId == tareaId).Select(a => a.Orden).MaxAsync();
            }

            var resultados = await _almacenadorArchivos.Almacenar(contenedor, archivos);

            var archivosAdjuntos = resultados.Select((resultado, indice) => new ArchivoAdjunto
            {
                TareaId = tareaId,
                FechaCreacion = DateTime.UtcNow,
                Url = resultado.URL,
                Titulo = resultado.Titulo,
                Orden = ordenMayor + indice + 1

            }).ToList();

            _context.AddRange(archivosAdjuntos);
            await _context.SaveChangesAsync();

            return archivosAdjuntos.ToList();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, string titulo)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            
            var archivoAdjunto = await _context.ArchivosAdjuntos.Include(a => a.Tarea)
                    .FirstOrDefaultAsync(a => a.Id == id);
            
            if (archivoAdjunto is null)
            {
                return NotFound();
            }

            if (archivoAdjunto.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            archivoAdjunto.Titulo= titulo;
            await _context.SaveChangesAsync();  

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();

            var archivoAdjunto = await _context.ArchivosAdjuntos.Include(a => a.Tarea)
                    .FirstOrDefaultAsync(a => a.Id == id);

            if (archivoAdjunto is null)
            {
                return NotFound();
            }

            if (archivoAdjunto.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            _context.Remove(archivoAdjunto); 
            await _context.SaveChangesAsync();
            await _almacenadorArchivos.Borrar(archivoAdjunto.Url, contenedor);


            return Ok();
        }
    }
}
