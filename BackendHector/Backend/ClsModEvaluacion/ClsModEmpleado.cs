using System;

namespace ClbModEvaluacion
{
    public class ClsModEmpleado 
    {
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public int IdPuesto { get; set; }
        public string NombrePuesto { get; set; }
    }
} 