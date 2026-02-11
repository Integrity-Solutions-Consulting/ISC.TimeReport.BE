namespace isc.time.report.be.api.Security
{
    public class ModuleSecurityOptions
    {
        public Dictionary<string, List<string>> RoleModules { get; set; } 
        public Dictionary<string, List<string>> ModuleRoutes { get; set; } 
        public List<string> IgnoreRoutes { get; set; } 
    }
}
