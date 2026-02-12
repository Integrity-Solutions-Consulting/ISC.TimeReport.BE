namespace isc.time.report.be.api.Security
{
    public class ModuleSecurityOptions
    {
        public Dictionary<string, List<string>> RoleModules { get; set; } 
        public Dictionary<string, List<string>> ModuleRoutes { get; set; } 
        public List<string> IgnoreRoutes { get; set; }

        public ResourceScopeOptions ResourceScope { get; set; }
    }
    public class ResourceScopeOptions
    {
        public List<string> SelfOnlyRoles { get; set; }
        public List<string> FullAccessRoles { get; set; }
    }
}
