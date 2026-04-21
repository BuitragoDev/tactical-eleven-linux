#nullable enable

namespace TacticalEleven.Scripts
{
    public class Manager
    {
        // Atributos
        public int IdManager { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Nacionalidad { get; set; } = null!;
        public string FechaNacimiento { get; set; } = null!;
        public int? IdEquipo { get; set; } // Puede ser null
        public int CDirectiva { get; set; }
        public int CFans { get; set; }
        public int CJugadores { get; set; }
        public int Reputacion { get; set; }
        public int PartidosJugados { get; set; }
        public int PartidosGanados { get; set; }
        public int PartidosEmpatados { get; set; }
        public int PartidosPerdidos { get; set; }
        public int Puntos { get; set; }
        public string Tactica { get; set; } = null!;
        public int Despedido { get; set; }
        public int PrimeraTemporada { get; set; }

        public string RutaImagen { get; set; } = null!;

        // Constructor por defecto
        public Manager() { }

        // Constructor con parámetros
        public Manager(int idManager, string nombre, string apellido, string nacionalidad, string fechaNacimiento, int? idEquipo,
            int cDirectiva, int cFans, int cJugadores, int reputacion, int partidosJugados, int partidosGanados, int partidosEmpatados,
            int partidosPerdidos, int puntos, string tactica, int despedido, string rutaImagen, int primeraTemporada)
        {
            IdManager = idManager;
            Nombre = nombre;
            Apellido = apellido;
            Nacionalidad = nacionalidad;
            FechaNacimiento = fechaNacimiento;
            IdEquipo = idEquipo;
            CDirectiva = cDirectiva;
            CFans = cFans;
            CJugadores = cJugadores;
            Reputacion = reputacion;
            PartidosJugados = partidosJugados;
            PartidosGanados = partidosGanados;
            PartidosEmpatados = partidosEmpatados;
            PartidosPerdidos = partidosPerdidos;
            Puntos = puntos;
            Tactica = tactica;
            Despedido = despedido;
            RutaImagen = rutaImagen;
            PrimeraTemporada = primeraTemporada;
        }

        // Constructor con parámetros sin ID_MANAGER
        public Manager(string nombre, string apellido, string nacionalidad, string fechaNacimiento, int? idEquipo, int cDirectiva,
            int cFans, int cJugadores, int reputacion, int partidosJugados, int partidosGanados, int partidosEmpatados,
            int partidosPerdidos, int puntos, string tactica, int despedido, string rutaImagen, int primeraTemporada)
        {
            Nombre = nombre;
            Apellido = apellido;
            Nacionalidad = nacionalidad;
            FechaNacimiento = fechaNacimiento;
            IdEquipo = idEquipo;
            CDirectiva = cDirectiva;
            CFans = cFans;
            CJugadores = cJugadores;
            Reputacion = reputacion;
            PartidosJugados = partidosJugados;
            PartidosGanados = partidosGanados;
            PartidosEmpatados = partidosEmpatados;
            PartidosPerdidos = partidosPerdidos;
            Puntos = puntos;
            Tactica = tactica;
            Despedido = despedido;
            RutaImagen = rutaImagen;
            PrimeraTemporada = primeraTemporada;
        }
    }
}