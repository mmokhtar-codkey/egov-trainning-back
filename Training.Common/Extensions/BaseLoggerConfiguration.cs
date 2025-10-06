using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class BaseLoggerConfiguration
    {
        public static LoggerConfiguration CreateLoggerConfiguration(string applicationName, string applicationVersion)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("ApplicationVersion", applicationVersion);
            return loggerConfiguration;
        }

        public static LoggerConfiguration WriteToSql(this LoggerConfiguration loggerConfiguration, string connectionString)
        {
            var sinkOpts = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            };

            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new() { ColumnName = "ApplicationName", DataType = SqlDbType.NVarChar, DataLength = 64, AllowNull = true },
                    new() { ColumnName = "ApplicationVersion", DataType = SqlDbType.NVarChar, DataLength = 32, AllowNull = true }
                }
            };
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.TimeStamp.NonClusteredIndex = true;
            loggerConfiguration.WriteTo.MSSqlServer(connectionString, sinkOpts, columnOptions: columnOptions);
            return loggerConfiguration;
        }


        //public static LoggerConfiguration WriteToSeq(this LoggerConfiguration loggerConfiguration, string seqServerUrl)
        //{
        //    loggerConfiguration.WriteTo.Seq(seqServerUrl);
        //    return loggerConfiguration;
        //}
    }
}
