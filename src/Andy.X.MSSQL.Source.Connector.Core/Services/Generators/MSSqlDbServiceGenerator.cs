﻿using Andy.X.MSSQL.Source.Connector.Core.Configurations;
using Andy.X.MSSQL.Source.Connector.Core.Utilities.Logging;
using Andy.X.MSSQL.Source.Connector.IO.Locations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.Data.SqlClient;
using System.Reflection;

namespace Andy.X.MSSQL.Source.Connector.Core.Services.Generators
{
    public class MSSqlDbServiceGenerator
    {
        private readonly MSSQLConfiguration dbEngineConfiguration;
        private bool isModelReturned = true;

        private string codeGenerationTemplate;
        private string activeCodeGeneratedTemplate;

        public MSSqlDbServiceGenerator(MSSQLConfiguration dbEngineConfiguration)
        {
            this.dbEngineConfiguration = dbEngineConfiguration;
        }

        public void CreateMSSqlTableModels()
        {
            codeGenerationTemplate = File.ReadAllText(AppLocations.GetServiceCodeGeneratorFile());

            string sqlTemplate = File.ReadAllText(AppLocations.GetSqlModelGeneratorFile());
            string sqlCurrentTableTemplate = "";

            foreach (var engine in dbEngineConfiguration.Servers)
            {
                foreach (var database in engine.Databases)
                {
                    foreach (var table in database.Tables)
                    {
                        activeCodeGeneratedTemplate = codeGenerationTemplate.Replace("{entity}", table.Name);


                        isModelReturned = false;
                        sqlCurrentTableTemplate = sqlTemplate.Replace("{TABLE_NAME}", table.Name);
                        ConvertSqlTableToCSharpModel(engine, database.Name, table.Name, sqlCurrentTableTemplate);

                        while (isModelReturned != true)
                        {
                            Thread.Sleep(300);
                        }

                        Logger.LogInformation($"Schema for table {engine.Name}.{database.Name}.{table.Name} is creating");

                        // Generate dll file for Service
                        string code = activeCodeGeneratedTemplate;
                        BuildSqlDbServiceDllFile(engine, database.Name, table.Name, code);
                        Logger.LogInformation($"Schema for table {engine.Name}.{database.Name}.{table.Name} is created successfully");
                    }
                }
            }
        }

        private void ConvertSqlTableToCSharpModel(Server engine, string database, string table, string command)
        {
            SqlConnection connect = new SqlConnection(engine.ConnectionString);
            connect.Open();

            var cmd = new SqlCommand(command, connect);
            connect.InfoMessage += Connect_InfoMessage;
            var result = cmd.ExecuteNonQuery();
            connect.Close();
        }

        private void Connect_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            activeCodeGeneratedTemplate = activeCodeGeneratedTemplate.Replace("{datamodel}", e.Message);
            isModelReturned = true;
        }

        private void BuildSqlDbServiceDllFile(Server engine, string database, string table, string code)
        {
            string serviceName = $"Andy.X.{engine.Name}.{database}.{table}.Service";
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            CSharpCompilation compilation = CSharpCompilation.Create(
                serviceName,
                new[] { syntaxTree },
                GetAssemblyReferences().ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));


            var fileName = $"{serviceName}.dll";
            var emitResult = compilation.Emit(fileName);
            if (!emitResult.Success)
            {
                Logger.LogError($"There is an error on generating schema for {database}.{table}, please check the database and table name, details are below");

                foreach (var error in emitResult.Diagnostics)
                {
                    Logger.LogError($"Error on schema for {database}.{table}, details={error}");
                }
            }
        }

        private IEnumerable<MetadataReference> GetAssemblyReferences()
        {
            var returnList = new List<MetadataReference>();

            //The location of the .NET assemblies
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var coreDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Private.CoreLib.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Private.Uri.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Console.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")));
            returnList.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Collections.dll")));
            //System.Collections

            // Adding Buildersoft related assemblies
            foreach (var file in AppLocations.GetAssemblyFiles())
            {
                returnList.Add(MetadataReference.CreateFromFile(file));
            }

            return returnList;
        }
    }
}
