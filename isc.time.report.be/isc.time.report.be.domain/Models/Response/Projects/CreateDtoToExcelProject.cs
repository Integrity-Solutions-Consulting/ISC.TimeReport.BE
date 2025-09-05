using isc.time.report.be.domain.Entity.Catalogs;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Models.Response.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.domain.Models.Response.Projects
{
    public class CreateDtoToExcelProject
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public int ProjectStatusID { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public int? ProjectTypeID { get; set; }
        public ProjectType ProjectType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Hours { get; set; }
        public bool? Status { get; set; }
        public DateTime? WaitingStartDate { get; set; }
        public DateTime? WaitingEndDate { get; set; }
        public string? Observation { get; set; }
        public List<LiderData>? LiderData { get; set; }
        public List<ClientData>? ClientData { get; set; }
    }
    public class LiderData
    {
        public int Id { get; set; }
        public GetPersonResponse GetPersonResponse { get; set; }
    }

    public class ClientData
    {
        public string TradeName { get; set; }
    }
    
}
