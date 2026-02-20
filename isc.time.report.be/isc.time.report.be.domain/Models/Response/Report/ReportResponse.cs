namespace isc.time.report.be.domain.Models.Response.Report
{
    public class ReportResponse
    {

        public record ProjectResourcesReportDto(
             string ClientName,
             DateTime StartDate,
             DateTime EndDate,
             string ProjectLeader,
             string ResourceName,
             string Position
        );

        public record ClientHourlyResourceAmountDto(
            string Client,
            int MonthNumber,
            int Year,
            int ResourceCount,
            decimal TotalHours
        );


    }
}
