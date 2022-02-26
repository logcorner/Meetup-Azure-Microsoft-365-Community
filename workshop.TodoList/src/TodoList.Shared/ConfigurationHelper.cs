using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TodoList.SharedKernel.WebApi
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(string userSecretsKey = null)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (!string.IsNullOrWhiteSpace(envName))
                builder.AddJsonFile($"appsettings.{envName}.json", optional: true);
            if (!string.IsNullOrWhiteSpace(userSecretsKey))
                builder.AddUserSecrets(userSecretsKey);
            builder.AddEnvironmentVariables();
            return builder.Build();
        }
    }
}