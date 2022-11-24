namespace Andy.X.MSSQL.Source.Connector.Core.Configurations
{
    public class AndyXConfiguration
    {
        public string[] ServiceUrls { get; set; }
        
        public string Tenant { get; set; }
        public string? TenantKey { get; set; }
        public string? TenantSecret { get; set; }
        
        public string Product { get; set; }
        public string? ProductKey { get; set; }
        public string? ProductSecret { get; set; }

        public string Component { get; set; }
        public string? ComponentKey { get; set; }
        public string? ComponentSecret { get; set; }
    }
}
