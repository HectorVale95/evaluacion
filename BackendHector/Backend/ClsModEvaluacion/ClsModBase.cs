using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClbModEvaluacion
{
    public class ClsModBase
    {
        [Range(0, 100000)]
        public int IdEmpresa { get; set; }

        [Range(0, 100000)]
        public int IdModulo { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaModificacion { get; set; }

        [Range(0, 100000)]
        public int IdUsuarioCreacion { get; set; }

        [Range(0, 100000)]
        public int IdUsuarioModificacion { get; set; }

        [Range(0, 100000)]
        public int IdUsuarioEliminado { get; set; }

        [Range(0, 1)]
        public int Eliminado { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaEliminado { get; set; }

        [Range(0, 100000)]
        public int IdTablaRelacion { get; set; }

        [Range(0, 100000)]
        public int IdRelacion { get; set; }

        [Range(0, 99999)]
        public int IdUsuario { get; set; }
    }
}
