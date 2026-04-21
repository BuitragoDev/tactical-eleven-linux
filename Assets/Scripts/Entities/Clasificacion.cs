#nullable enable

namespace TacticalEleven.Scripts
{
    public class Clasificacion
    {
        public int IdEquipo { get; set; }
        public int Jugados { get; set; }
        public int Ganados { get; set; }
        public int Empatados { get; set; }
        public int Perdidos { get; set; }
        public int Puntos { get; set; }
        public int LocalVictorias { get; set; }
        public int LocalDerrotas { get; set; }
        public int VisitanteVictorias { get; set; }
        public int VisitanteDerrotas { get; set; }
        public int GolesFavor { get; set; }
        public int GolesContra { get; set; }
        public int Racha { get; set; }

        // Atributos extra
        public int Posicion { get; set; }
        public string? NombreEquipo { get; set; }
        public string? ColumnaAuxiliar { get; set; } // Nueva propiedad para la columna vacía

        // Constructor vacío
        public Clasificacion()
        {

        }

        // Constructor con parámetros
        public Clasificacion(int idEquipo, int jugados, int ganados, int empatados, int perdidos, int puntos,
                             int localVictorias, int localDerrotas, int visitanteVictorias, int visitanteDerrotas,
                             int golesFavor, int golesContra, int racha)
        {
            IdEquipo = idEquipo;
            Jugados = jugados;
            Ganados = ganados;
            Empatados = empatados;
            Perdidos = perdidos;
            Puntos = puntos;
            LocalVictorias = localVictorias;
            LocalDerrotas = localDerrotas;
            VisitanteVictorias = visitanteVictorias;
            VisitanteDerrotas = visitanteDerrotas;
            GolesFavor = golesFavor;
            GolesContra = golesContra;
            Racha = racha;
        }

        // Método ToString
        public override string ToString()
        {
            return $"Equipo: {IdEquipo}, Jugados: {Jugados}, Ganados: {Ganados}, Empatados: {Empatados}, Perdidos: {Perdidos}, " +
                   $"Local Victorias: {LocalVictorias}, Local Derrotas: {LocalDerrotas}, VisitanteVictorias: {VisitanteVictorias}, VisitanteDerrotas: {VisitanteDerrotas}," +
                   $"GolesFavor: {GolesFavor}, GolesContra: {GolesContra}, Racha: {Racha}, Puntos: {Puntos}";
        }
    }
}