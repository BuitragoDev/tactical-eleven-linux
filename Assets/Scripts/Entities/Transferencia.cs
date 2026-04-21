using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class Transferencia
    {
        public int IdFichaje { get; set; }
        public int IdJugador { get; set; }
        public int IdEquipoOrigen { get; set; }
        public int IdEquipoDestino { get; set; }
        public int TipoFichaje { get; set; }
        public int MontoOferta { get; set; }
        public string FechaOferta { get; set; }
        public string FechaTraspaso { get; set; }
        public int SalarioAnual { get; set; }
        public string FinContrato { get; set; }
        public int ClausulaRescision { get; set; }
        public int BonoPorGoles { get; set; }
        public int BonoPorPartidos { get; set; }
        public int? RespuestaEquipo { get; set; }
        public int? RespuestaJugador { get; set; }
        public int ValorMercado { get; set; }
        public int SituacionMercado { get; set; }
        public int Moral { get; set; }
        public int EstadoAnimo { get; set; }
        public int Status { get; set; }
        public int PresupuestoComprador { get; set; }
        public int PresupuestoVendedor { get; set; }
        public int Rival { get; set; }
        public int Duracion { get; set; }

        // Constructor vacío
        public Transferencia() { }

        // Constructor con parámetros
        public Transferencia(int idFichaje, int idJugador, int idEquipoOrigen, int idEquipoDestino, int tipoFichaje,
                              int montoOferta, string fechaOferta, string fechaTraspaso, int salarioAnual, string finContrato, int clausulaRescision,
                              int bonoPorGoles, int bonoPorPartidos, int? respuestaEquipo, int? respuestaJugador,
                              int valorMercado, int situacionMercado, int moral, int estadoAnimo, int status, int presupuestoComprador,
                              int presupuestoVendedor, int rival, int duracion)
        {
            IdFichaje = idFichaje;
            IdJugador = idJugador;
            IdEquipoOrigen = idEquipoOrigen;
            IdEquipoDestino = idEquipoDestino;
            TipoFichaje = tipoFichaje;
            MontoOferta = montoOferta;
            FechaOferta = fechaOferta;
            FechaTraspaso = fechaTraspaso;
            SalarioAnual = salarioAnual;
            FinContrato = finContrato;
            ClausulaRescision = clausulaRescision;
            BonoPorGoles = bonoPorGoles;
            BonoPorPartidos = bonoPorPartidos;
            RespuestaEquipo = respuestaEquipo;
            RespuestaJugador = respuestaJugador;
            ValorMercado = valorMercado;
            SituacionMercado = situacionMercado;
            Moral = moral;
            EstadoAnimo = estadoAnimo;
            Status = status;
            PresupuestoComprador = presupuestoComprador;
            PresupuestoVendedor = presupuestoVendedor;
            Rival = rival;
            Duracion = duracion;
        }

        // Constructor con parámetros sin ID
        public Transferencia(int idJugador, int idEquipoOrigen, int idEquipoDestino, int tipoFichaje,
                              int montoOferta, string fechaOferta, string fechaTraspaso, int salarioAnual, string finContrato, int clausulaRescision,
                              int bonoPorGoles, int bonoPorPartidos, int? respuestaEquipo, int? respuestaJugador,
                              int valorMercado, int situacionMercado, int moral, int estadoAnimo, int status, int presupuestoComprador,
                              int presupuestoVendedor, int rival, int duracion)
        {
            IdJugador = idJugador;
            IdEquipoOrigen = idEquipoOrigen;
            IdEquipoDestino = idEquipoDestino;
            TipoFichaje = tipoFichaje;
            MontoOferta = montoOferta;
            FechaOferta = fechaOferta;
            FechaTraspaso = fechaTraspaso;
            SalarioAnual = salarioAnual;
            FinContrato = finContrato;
            ClausulaRescision = clausulaRescision;
            BonoPorGoles = bonoPorGoles;
            BonoPorPartidos = bonoPorPartidos;
            RespuestaEquipo = respuestaEquipo;
            RespuestaJugador = respuestaJugador;
            ValorMercado = valorMercado;
            SituacionMercado = situacionMercado;
            Moral = moral;
            EstadoAnimo = estadoAnimo;
            Status = status;
            PresupuestoComprador = presupuestoComprador;
            PresupuestoVendedor = presupuestoVendedor;
            Rival = rival;
            Duracion = duracion;
        }

        public override string ToString()
        {
            return $"IdFichaje: {IdFichaje}, IdJugador: {IdJugador}, IdEquipoOrigen: {IdEquipoOrigen}, IdEquipoDestino: {IdEquipoDestino}, " +
                   $"TipoFichaje: {TipoFichaje}, MontoOferta: {MontoOferta}, FechaOferta: {FechaOferta}, Fecha Traspaso: {FechaTraspaso}, SalarioAnual: {SalarioAnual}, " +
                   $"FinContrato: {FinContrato}, ClausulaRescision: {ClausulaRescision}, BonoPorGoles: {BonoPorGoles}, BonoPorPartidos: {BonoPorPartidos}, " +
                   $"RespuestaEquipo: {RespuestaEquipo?.ToString() ?? "N/A"}, RespuestaJugador: {RespuestaJugador?.ToString() ?? "N/A"}, " +
                   $"ValorMercado: {ValorMercado}, SituacionMercado: {SituacionMercado}, Moral: {Moral}, EstadoAnimo: {EstadoAnimo}, " +
                   $"Status: {Status}, PresupuestoComprador: {PresupuestoComprador}, PresupuestoVendedor: {PresupuestoVendedor}, " +
                   $"Rival: {Rival}, Duracion: {Duracion}";
        }
    }
}
