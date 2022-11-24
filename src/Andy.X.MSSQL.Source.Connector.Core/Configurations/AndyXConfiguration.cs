namespace Andy.X.MSSQL.Source.Connector.Core.Configurations
{
    public class AndyXConfiguration
    {
        public string[] ServiceUrls { get; set; }
        public string Tenant { get; set; }
        public string Product { get; set; }
        public string Component { get; set; }

        public string TenantToken { get; set; }
        public string ComponentToken { get; set; }
    }
}
