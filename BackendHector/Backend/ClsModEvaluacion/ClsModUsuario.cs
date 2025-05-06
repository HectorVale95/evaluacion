using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ClbModEvaluacion
{
    [DataContract]
    public class ClsModUsuario
    {
        [DataMember]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [DataMember(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [DataMember]
        public string Nombre { get; set; } = string.Empty;

        [DataMember]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [DataMember]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [DataMember]
        public string Role { get; set; } = string.Empty;

        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public DateTime FechaCreacion { get; set; }

        // Constructor para inicializar las propiedades string
        public ClsModUsuario()
        {
            Username = string.Empty;
            Nombre = string.Empty;
            ApellidoPaterno = string.Empty;
            ApellidoMaterno = string.Empty;
            Role = string.Empty;
            Activo = false;
            FechaCreacion = DateTime.MinValue;
        }

        // Método para depuración
        public override string ToString()
        {
            return $"IdUsuario: {IdUsuario}, Username: '{Username}', Role: '{Role}', Activo: {Activo}";
        }
    }
}
