namespace TacticalEleven.Scripts
{
    public class Historial
    {
        public int IdHistorial { get; set; }
        public int IdEquipo { get; set; }
        public int IdManager { get; set; }
        public string Temporada { get; set; }
        public int PosicionLiga { get; set; }
        public int PartidosJugados { get; set; }
        public int PartidosGanados { get; set; }
        public int PartidosEmpatados { get; set; }
        public int PartidosPerdidos { get; set; }
        public int GolesMarcados { get; set; }
        public int GolesRecibidos { get; set; }
        public int TitulosInternacionales { get; set; }
        public int CDirectiva { get; set; }
        public int CFans { get; set; }
        public int CJugadores { get; set; }

        // Atributos extra
        public string NombreEquipo { get; set; }

        // Constructor por defecto
        public Historial() { }

        // Constructor con parámetros
        public Historial(int idHistorial, int idEquipo, int idManager, string temporada, int posicionLiga,
                            int partidosJugados, int partidosGanados, int partidosEmpatados, int partidosPerdidos,
                            int golesMarcados, int golesRecibidos, int titulosInternacionales,
                            int cDirectiva, int cFans, int cJugadores)
        {
            IdHistorial = idHistorial;
            IdEquipo = idEquipo;
            IdManager = idManager;
            Temporada = temporada;
            PosicionLiga = posicionLiga;
            PartidosJugados = partidosJugados;
            PartidosGanados = partidosGanados;
            PartidosEmpatados = partidosEmpatados;
            PartidosPerdidos = partidosPerdidos;
            GolesMarcados = golesMarcados;
            GolesRecibidos = golesRecibidos;
            TitulosInternacionales = titulosInternacionales;
            CDirectiva = cDirectiva;
            CFans = cFans;
            CJugadores = cJugadores;
        }

        // Constructor con parámetros SIN ID
        public Historial(int idEquipo, int idManager, string temporada, int posicionLiga,
                            int partidosJugados, int partidosGanados, int partidosEmpatados, int partidosPerdidos,
                            int golesMarcados, int golesRecibidos, int titulosInternacionales,
                            int cDirectiva, int cFans, int cJugadores)
        {
            IdEquipo = idEquipo;
            IdManager = idManager;
            Temporada = temporada;
            PosicionLiga = posicionLiga;
            PartidosJugados = partidosJugados;
            PartidosGanados = partidosGanados;
            PartidosEmpatados = partidosEmpatados;
            PartidosPerdidos = partidosPerdidos;
            GolesMarcados = golesMarcados;
            GolesRecibidos = golesRecibidos;
            TitulosInternacionales = titulosInternacionales;
            CDirectiva = cDirectiva;
            CFans = cFans;
            CJugadores = cJugadores;
        }

        // Método ToString
        public override string ToString()
        {
            return $"ID: {IdHistorial}, Temporada: {Temporada}, Equipo: {IdEquipo}, Manager: {IdManager}, " +
                    $"Posición: {PosicionLiga}, PJ: {PartidosJugados}, PG: {PartidosGanados}, PE: {PartidosEmpatados}, " +
                    $"PP: {PartidosPerdidos}, GF: {GolesMarcados}, GC: {GolesRecibidos}, " +
                    $"Titulos Internacionales: {TitulosInternacionales}";
        }
    }
}