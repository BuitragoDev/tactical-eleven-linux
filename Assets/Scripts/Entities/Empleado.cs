#nullable enable

namespace TacticalEleven.Scripts
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string? Nombre { get; set; }
        public string? Puesto { get; set; }
        public int Categoria { get; set; }
        public int Salario { get; set; }
        public int? IdManager { get; set; }
        public int? IdEquipo { get; set; }

        // Constructor sin parámetros
        public Empleado() { }

        // Constructor con parámetros
        public Empleado(int idEmpleado, string nombre, string puesto, int categoria,
                         int salario, int? idManager, int? idEquipo)
        {
            IdEmpleado = idEmpleado;
            Nombre = nombre;
            Puesto = puesto;
            Categoria = categoria;
            Salario = salario;
            IdManager = idManager;
            IdEquipo = idEquipo;
        }

        // Constructor con parámetros sin ID
        public Empleado(string nombre, string puesto, int categoria,
                         int salario, int? idManager, int? idEquipo)
        {
            Nombre = nombre;
            Puesto = puesto;
            Categoria = categoria;
            Salario = salario;
            IdManager = idManager;
            IdEquipo = idEquipo;
        }

        // Método ToString para mostrar la información del empleado
        public override string ToString()
        {
            return $"ID Empleado: {IdEmpleado}, Nombre: {Nombre}, Puesto: {Puesto}, " +
                   $"Categoría: {Categoria}, Salario: {Salario} " +
                   $"ID Manager: {IdManager ?? -1}, ID Equipo: {IdEquipo ?? -1}";
        }
    }
}