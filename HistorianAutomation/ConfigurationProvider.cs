using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistorianAutomation
{
    public class ConfigurationProvider
    {
        private static String EnvironmentKey = "env";
        static String ApiEnvironmentKey = "apienv";

        public static Dictionary<string, string> GetConfiguration()
        {
            
            var appConfigFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\applicationconfig.json";
            var appConfigText = File.ReadAllText(appConfigFile);

            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(appConfigText);

            var env = TestConfiguration.GetConfigValue("env");
            
            if (TestConfiguration.DoesEnvVariableExist(EnvironmentKey))
            {
                var sub = TestConfiguration.GetConfigValue("subpath");

                config[config[EnvironmentKey]] = env + (TestConfiguration.DoesEnvVariableExist(EnvironmentKey) ? sub : "") + "historian";
                config[config[ApiEnvironmentKey]] = env;

                Console.WriteLine($"Api client is using url: {config[config[EnvironmentKey]]}");
                Console.WriteLine($"Http client is using url: {config[config[ApiEnvironmentKey]]}");
            }
            return config;
        }

        public static Dictionary<string, string> GetCustomConfiguration(string customFileName)
        {
            var appConfigFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + $"\\CustomTests\\Configuration\\{customFileName}.json";
            var appConfigText = File.ReadAllText(appConfigFile);

            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(appConfigText);
            var env = TestConfiguration.GetConfigValue("env");

            if (!String.IsNullOrEmpty(env))
            {
                config[config[EnvironmentKey]] = env + "historian";
                config[config[ApiEnvironmentKey]] = env;

                Console.WriteLine($"Api client is using url: {config[config[EnvironmentKey]]}");
                Console.WriteLine($"Http client is using url: {config[config[ApiEnvironmentKey]]}");
            }

            return config;
        }
    }

    public class TestConfiguration
    {

        public static bool DoesEnvVariableExist(string variable)
        {
            return !String.IsNullOrEmpty(TestContext.Parameters[variable]);
        }

        public static string GetConfigValue(string configVariable)
        {
            return Environment.GetEnvironmentVariable(configVariable) ?? TestContext.Parameters[configVariable];
        }
    }
}
