using Andy.X.MSSQL.Source.Connector.Core.Configurations;
using Andy.X.MSSQL.Source.Connector.Core.Configurations.Abstraction.Services;
using Andy.X.MSSQL.Source.Connector.Core.Services.Generators;
using Andy.X.MSSQL.Source.Connector.Core.Utilities.Extensions;
using Andy.X.MSSQL.Source.Connector.Core.Utilities.Logging;
using Andy.X.MSSQL.Source.Connector.IO.Locations;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;

namespace Andy.X.MSSQL.Source.Connector.Core.Services
{
    public class GlobalService
    {
        private AndyXConfiguration andyXConfiguration;
        private MSSQLConfiguration dbEngineConfiguration;
        private bool isDbEngineConfigImported;

        private ConcurrentDictionary<string, ISqlDbTableService> sqlDbTableServices;
        private MSSqlDbServiceGenerator sqlDbServiceGenerator;

        public GlobalService()
        {
            isDbEngineConfigImported = false;

            ReadConfigurationFiles();
            CreateAndInitializeDbEngineServices();
        }

        private void ReadConfigurationFiles()
        {
            Logger.LogInformation("Importing configuration files");
            if (Directory.Exists(AppLocations.ConfigDirectory()) != true)
            {
                Logger.LogError($"Importing configuration files failed, config directory does not exists; path={AppLocations.ConfigDirectory()}");
                Directory.CreateDirectory(AppLocations.ConfigDirectory());
                Logger.LogInformation($"Importing configuration files failed, config directory created; path={AppLocations.ConfigDirectory()}");
            }

            if (Directory.Exists(AppLocations.ServicesDirectory()) != true)
                Directory.CreateDirectory(AppLocations.ServicesDirectory());

            if (Directory.Exists(AppLocations.TemplatesDirectory()) != true)
                Directory.CreateDirectory(AppLocations.TemplatesDirectory());

            // checking if files exits
            if (File.Exists(AppLocations.GetAndyXConfigurationFile()) != true)
            {
                Logger.LogError($"Importing configuration files failed, andyx_config.json file does not exists; path={AppLocations.GetAndyXConfigurationFile()}");
                throw new Exception($"ANDYX-CONNECT|[error]|importing|andyx_config.json|file not exists|path={AppLocations.GetAndyXConfigurationFile()}");
            }

            // checking if dbengines files exits
            if (File.Exists(AppLocations.GetDbEnginesConfigurationFile()) == true)
            {
                isDbEngineConfigImported = true;
                dbEngineConfiguration = File.ReadAllText(AppLocations.GetDbEnginesConfigurationFile()).JsonToObject<MSSQLConfiguration>();
                Logger.LogInformation($"Database engines are imported successfully");
            }
            else
            {
                Logger.LogWarning($"Importing database engine file configuration is skipped, server_config.json file does not exists; path={AppLocations.GetDbEnginesConfigurationFile()}");
            }

            andyXConfiguration = File.ReadAllText(AppLocations.GetAndyXConfigurationFile()).JsonToObject<AndyXConfiguration>();
            Logger.LogInformation($"Andy X configuration settings are imported successfully");
        }

        private void CreateAndInitializeDbEngineServices()
        {
            if (isDbEngineConfigImported is true)
            {
                CreateMSSqlServices();
                InitializeEngineDbTableServices();
            }
        }

        private void CreateMSSqlServices()
        {
            sqlDbServiceGenerator = new MSSqlDbServiceGenerator(dbEngineConfiguration);
            sqlDbServiceGenerator.CreateMSSqlTableModels();
        }

        private void InitializeEngineDbTableServices()
        {
            sqlDbTableServices = new ConcurrentDictionary<string, ISqlDbTableService>();

            foreach (var engine in dbEngineConfiguration.Servers)
            {
                InitializeMSSQLServices(engine);
            }
        }

        private void InitializeMSSQLServices(Server engine)
        {
            foreach (var database in engine.Databases)
            {
                foreach (var table in database.Tables)
                {
                    var tableWorker = AssemblyLoadContext.Default.
                        LoadFromAssemblyPath(AppLocations.GetDbServiceAssemblyFile(engine.Name, database.Name, table.Name));
                    var serviceType = tableWorker.GetType("Andy.X.Connect.MSSQL.Code.Generated.Service.SqlDbTableService");

                    ConstructorInfo ctor = serviceType!.GetConstructor(new[] { typeof(string), typeof(string), typeof(Table), typeof(AndyXConfiguration) })!;
                    ISqlDbTableService? instance = ctor.Invoke(new object[] { engine.ConnectionString, database.Name, table, andyXConfiguration })
                        as ISqlDbTableService;

                    instance!.Connect();

                    Thread.Sleep(2000);

                    sqlDbTableServices.TryAdd($"MSSQL-{database.Name}-{table.Name}", instance);
                }
            }
        }
    }
}
