using System;
using System.Collections.Generic;

namespace TacticalEleven.Scripts
{
    public class DatosSimulacion
    {
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        
        public List<(Jugador jugador, Jugador?)> goleadoresLocal { get; set; }
        public List<(Jugador jugador, Jugador?)> goleadoresVisitante { get; set; }
        
        public List<(Jugador jugador, string tipoTarjeta)> tarjetasLocal { get; set; }
        public List<(Jugador jugador, string tipoTarjeta)> tarjetasVisitante { get; set; }
        
        public Jugador MVP { get; set; }
        public int GolesMVP { get; set; }
        public int AsistenciasMVP { get; set; }
        
        public int Asistencia { get; set; }
        public int Recaudacion { get; set; }
        
        public List<Jugador> JugadoresLocal { get; set; }
        public List<Jugador> JugadoresVisitante { get; set; }
    }
}