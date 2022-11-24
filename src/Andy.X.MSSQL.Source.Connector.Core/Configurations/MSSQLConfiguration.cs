namespace Andy.X.MSSQL.Source.Connector.Core.Configurations
{
    public class MSSQLConfiguration
    {
        public List<Server> Servers { get; set; }
    }

    public class Server
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public List<Database> Databases { get; set; }
    }

    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
    }

    public class Table
    {
        public string Name { get; set; }
        public bool IncludeOldVersion { get; set; }

        public string Topic { get; set; }

        public Table()
        {
            IncludeOldVersion = false;
        }
    }
}
