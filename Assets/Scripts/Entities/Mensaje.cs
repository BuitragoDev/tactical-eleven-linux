#nullable enable

using System;

namespace TacticalEleven.Scripts
{
    public class Mensaje
    {
        public int IdMensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string? Remitente { get; set; }
        public string? Asunto { get; set; }
        public string? Contenido { get; set; }
        public string? TipoMensaje { get; set; }
        public int? IdEquipo { get; set; } // Nullable, ya que puede ser NULL en la base de datos
        public bool Leido { get; set; }
        public int Icono { get; set; }

        // Constructor por defecto
        public Mensaje()
        {
        }

        // Constructor con parámetros
        public Mensaje(int idMensaje, DateTime fecha, string remitente, string asunto, string contenido, string tipoMensaje, int? idEquipo, bool leido, int icono)
        {
            IdMensaje = idMensaje;
            Fecha = fecha;
            Remitente = remitente;
            Asunto = asunto;
            Contenido = contenido;
            TipoMensaje = tipoMensaje;
            IdEquipo = idEquipo;
            Leido = leido;
            Icono = icono;
        }

        // Constructor sin ID (útil para insertar nuevos mensajes)
        public Mensaje(DateTime fecha, string remitente, string asunto, string contenido, string tipoMensaje, int? idEquipo, bool leido, int icono)
        {
            Fecha = fecha;
            Remitente = remitente;
            Asunto = asunto;
            Contenido = contenido;
            TipoMensaje = tipoMensaje;
            IdEquipo = idEquipo;
            Leido = leido;
            Icono = icono;
        }

        // Método ToString para representar el mensaje
        public override string ToString()
        {
            return $"ID: {IdMensaje}, Fecha: {Fecha}, Remitente: {Remitente}, Asunto: {Asunto}, " +
                   $"Contenido: {Contenido}, Tipo: {TipoMensaje}, Equipo: {IdEquipo}, Leído: {Leido}";
        }
    }
}