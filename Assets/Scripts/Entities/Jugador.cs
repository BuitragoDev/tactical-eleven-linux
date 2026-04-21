#nullable enable

using System;

namespace TacticalEleven.Scripts
{
    public class Jugador
    {
        public int IdJugador { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreCompleto => $"{Nombre} {Apellido}";
        public int IdEquipo { get; set; }
        public int Dorsal { get; set; }
        public string? Rol { get; set; }
        public int RolId { get; set; }
        public int Velocidad { get; set; }
        public int Resistencia { get; set; }
        public int Agresividad { get; set; }
        public int Calidad { get; set; }
        public int EstadoForma { get; set; }
        public int EstadoAnimo { get; set; }
        public int Moral { get; set; }
        public int Potencial { get; set; }
        public int Portero { get; set; }
        public int Pase { get; set; }
        public int Regate { get; set; }
        public int Remate { get; set; }
        public int Entradas { get; set; }
        public int Tiro { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Peso { get; set; }
        public int Altura { get; set; }
        public double AlturaEnMetros => Altura / 100.00; // Propiedad calculada para convertir altura
        public int Lesion { get; set; }
        public string TipoLesion { get; set; } = null!;
        public int LesionTratada { get; set; }
        public string? Nacionalidad { get; set; }
        public int Status { get; set; } // 1. Clave, 2. Importante, 3. Rotación, 4. Ocasional
        public int Entrenamiento { get; set; } // 1 Portero, 2 Entradas, 3 Remate, 4 Pase, 5 Regate, 6 Tiro
        public int Sancionado { get; set; }
        public string RutaImagen { get; set; } = null!;
        public int ValorMercado { get; set; }
        public int SituacionMercado { get; set; } // 1. Transferible, 2. Cedible, 3. Intransferible, 4. Cedido, 0. Por defecto

        // Atributos extra
        public string? NombreEquipo { get; set; }
        public int PosicionAlineacion { get; set; }
        public int Valoracion { get; set; }
        public string ValoracionTexto => $"{Valoracion} puntos";
        public int? AniosContrato { get; set; } // Puede ser null si no tiene contrato
        public int? SalarioTemporada { get; set; } // Puede ser null si no tiene salario registrado
        public int? ClausulaRescision { get; set; }
        public int? BonusPartido { get; set; }
        public int? BonusGoles { get; set; }
        public DateTime? ProximaNegociacion { get; set; }
        public string? FechaInforme { get; set; }

        // Propiedad calculada para obtener la EDAD
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento > hoy.AddYears(-edad)) edad--; // Ajustar si aún no ha cumplido años este año
                return edad;
            }
        }

        // Propiedad calculada para obtener la MEDIA
        public int Media
        {
            get
            {
                return (int)((Velocidad + Resistencia + Agresividad + Calidad + EstadoForma + Moral) / 6);
            }
        }

        // Constructor vacío
        public Jugador() { }

        // Constructor con parámetros
        public Jugador(int idJugador, string nombre, string apellido, int idEquipo, int dorsal, string rol, int rolId,
                       int velocidad, int resistencia, int agresividad, int calidad, int estadoForma, int estadoAnimo, int moral,
                       int potencial, int portero, int pase, int regate, int remate, int entradas, int tiro,
                       DateTime fechaNacimiento, int peso, int altura, int lesion, string tipoLesion, string nacionalidad, int status,
                       int entrenamiento, int sancionado, string rutaImagen, int lesionTratada)
        {
            IdJugador = idJugador;
            Nombre = nombre;
            Apellido = apellido;
            IdEquipo = idEquipo;
            Dorsal = dorsal;
            Rol = rol;
            RolId = rolId;
            Velocidad = velocidad;
            Resistencia = resistencia;
            Agresividad = agresividad;
            Calidad = calidad;
            EstadoForma = estadoForma;
            EstadoAnimo = estadoAnimo;
            Moral = moral;
            Potencial = potencial;
            Portero = portero;
            Pase = pase;
            Regate = regate;
            Remate = remate;
            Entradas = entradas;
            Tiro = tiro;
            FechaNacimiento = fechaNacimiento;
            Peso = peso;
            Altura = altura;
            Lesion = lesion;
            TipoLesion = tipoLesion;
            Nacionalidad = nacionalidad;
            Status = status;
            Entrenamiento = entrenamiento;
            Sancionado = sancionado;
            RutaImagen = rutaImagen;
            LesionTratada = lesionTratada;
        }

        // Constructor con parámetros SIN ID
        public Jugador(string nombre, string apellido, int idEquipo, int dorsal, string rol, int rolId,
                       int velocidad, int resistencia, int agresividad, int calidad, int estadoForma, int estadoAnimo, int moral,
                       int potencial, int portero, int pase, int regate, int remate, int entradas, int tiro,
                       DateTime fechaNacimiento, int peso, int altura, int lesion, string tipoLesion, string nacionalidad, int status,
                       int entrenamiento, int sancionado, string rutaImagen, int lesionTratada)
        {
            Nombre = nombre;
            Apellido = apellido;
            IdEquipo = idEquipo;
            Dorsal = dorsal;
            Rol = rol;
            RolId = rolId;
            Velocidad = velocidad;
            Resistencia = resistencia;
            Agresividad = agresividad;
            Calidad = calidad;
            EstadoForma = estadoForma;
            EstadoAnimo = estadoAnimo;
            Moral = moral;
            Potencial = potencial;
            Portero = portero;
            Pase = pase;
            Regate = regate;
            Remate = remate;
            Entradas = entradas;
            Tiro = tiro;
            FechaNacimiento = fechaNacimiento;
            Peso = peso;
            Altura = altura;
            Lesion = lesion;
            TipoLesion = tipoLesion;
            Nacionalidad = nacionalidad;
            Status = status;
            Entrenamiento = entrenamiento;
            Sancionado = sancionado;
            RutaImagen = rutaImagen;
            LesionTratada = lesionTratada;
        }

        // Método ToString
        public override string ToString()
        {
            return $"{NombreCompleto} ({Edad} años) - {Rol} en {NombreEquipo}. Media: {Media}";
        }
    }
}