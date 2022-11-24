namespace Andy.X.MSSQL.Source.Connector.Core.Configurations.Abstraction.Services
{
    public interface ISqlDbTableService
    {
        public void Connect();
        public void Disconnect();
    }
}
