using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Dto.Projects
{
    public class ProjectExcelDto
    {
        public int Nro { get; set; }
        public string CodigoProyecto { get; set; }
        public string Proyecto { get; set; }
        public string Lider { get; set; }
        public string Cliente { get; set; }
        public string EstadoProyecto { get; set; }
        public string TipoProyecto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinEstimada { get; set; }
        public DateTime? FechaFinReal { get; set; }
        public decimal Presupuesto { get; set; }
        public int Horas { get; set; }
        public DateTime? FechaInicioEspera { get; set; }
        public DateTime? FechaFinEspera { get; set; }
        public string Observaciones { get; set; }
    }

}
