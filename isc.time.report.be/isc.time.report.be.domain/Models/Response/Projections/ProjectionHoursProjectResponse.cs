using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projections
{
    public record ProjectionHoursProjectResponse(
        int IDRecurso,
        string TipoRecurso,
        string NombreRecurso,
        decimal CostoPorHora
    );
}
