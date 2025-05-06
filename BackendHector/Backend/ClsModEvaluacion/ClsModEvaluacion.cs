using System;

namespace ClbModEvaluacion
{
    public class ClsModEvaluacion 
    {
        public int IdEvaluacion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public int Productividad { get; set; }
        public int Puntualidad { get; set; }
        public int CalidadTrabajo { get; set; }
        public int Comunicacion { get; set; }
        public int DisposicionAprender { get; set; }
        public int Honestidad { get; set; }
        public int Iniciativa { get; set; }
        public int IntegracionEquipo { get; set; }
        public string Comentarios { get; set; }
        public int TotalPuntos { get; set; }
        public string NombreEmpleado { get; set; }
    }
} 
