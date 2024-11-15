using LogicMonitor.DataSDK.Api;
using LogicMonitor.DataSDK;
using Microsoft.Extensions.Logging;
using LogicMonitor.DataSDK.Model;
using System.Net;

namespace LMLogging
{
    public sealed class LMLogger : ILogger
    {
        private string company = "";      //  Company name; or portal preves  <company>.logicmonitor.com
        private string accessID = "";     //  LMv1 Access ID here
        private string accessKey = "";    //  LMv1 Access Key here
        private MyResponse responseInterface = new MyResponse();
        private Configuration config;
        Resource resource;
        //Pass the Authenticate Variables as Enviroment variable.
        private ApiClient apiClient;
        private static Logs logs;
        private LMLoggerConfiguration configuration;
        public LMLogger()
        {
            try
            {
                configuration = new LMLoggerConfiguration();
                var resourceName = Dns.GetHostName();
                Dictionary<string, string> resourceIds = new Dictionary<string, string>();
                resourceIds.Add("system.displayname", resourceName.ToString());

                config = new Configuration(company: company, accessID: accessID, accessKey: accessKey);
                apiClient = new ApiClient(config);
                resource = new Resource(name: resourceName.ToString(), ids: resourceIds, create: true);
                logs = new Logs(batch: false, interval: 0, responseCallback: responseInterface, apiClient: apiClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return !configuration.LogLevels.ContainsKey(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            logs.SendLogs(state.ToString(), resource: resource, logLevel: logLevel);
        }
    }

    public sealed class LMLoggerConfiguration
    {
        public Dictionary<LogLevel, bool> LogLevels { get;  set; } = new()
        {
            [LogLevel.Debug] = true
        };
    } 
}
