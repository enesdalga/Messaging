using System;
using System.IO;
using System.Linq;
using Messaging.API.Data;
using Messaging.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Messaging.API.Helpers
{
    public class FileLogger : ILogger
    {
        private readonly IHostingEnvironment env;

        public FileLogger(IHostingEnvironment env)
        {
            this.env = env;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Format("{0}: {1} - {2}", logLevel.ToString(), eventId.Id, formatter(state, exception));
            WriteToFile(message);
        }
        private void WriteToFile(string message)
        {
            string filePath = $"{env.ContentRootPath}/Log";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            using (var streamWriter = new StreamWriter($"{filePath}/log.txt", true))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
    }
}