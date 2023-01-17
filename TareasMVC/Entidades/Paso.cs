namespace TareasMVC.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; }

        public int TareaId { get; set; }

        // PROPIEDAD DE NAVEGACIÓN (se configura automaticamente como llave foranea)
        // Relación 1 a muchos
        public Tarea Tarea { get; set; }

        public string Descripcion { get; set; }

        public bool Realizado { get; set; }

        public int Orden { get; set; }
    }
}
