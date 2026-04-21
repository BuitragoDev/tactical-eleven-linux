#nullable enable

namespace TacticalEleven.Scripts
{
    public class Patrocinador
    {
        public int IdPatrocinador { get; set; }
        public string? Nombre { get; set; }
        public int Reputacion { get; set; }
        public int Cantidad { get; set; }
        public int Mensualidad { get; set; }
        public int DuracionContrato { get; set; }

        // Constructores
        public Patrocinador()
        {
        }

        public Patrocinador(int idPatrocinador, string nombre, int reputacion, int cantidad, int mensualidad, int duracionContrato)
        {
            IdPatrocinador = idPatrocinador;
            Nombre = nombre;
            Reputacion = reputacion;
            Cantidad = cantidad;
            DuracionContrato = duracionContrato;
            Mensualidad = mensualidad;
        }

        public Patrocinador(string nombre, int cantidad, int mensualidad, int duracionContrato)
        {
            Nombre = nombre;
            Cantidad = cantidad;
            Mensualidad = mensualidad;
            DuracionContrato = duracionContrato;
        }

        // ToString
        public override string ToString()
        {
            return $"Patrocinador: {Nombre}, Cantidad: {Cantidad}, Mensualidad: {Mensualidad}, Duración: {DuracionContrato} meses";
        }
    }
}