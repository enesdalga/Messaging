using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Messaging.API.Helpers
{
    public class FileLogProvider : ILoggerProvider
    {
        private readonly IHostingEnvironment env;

        public FileLogProvider(IHostingEnvironment env)
        {
            this.env = env;
        }
        public ILogger CreateLogger(string category)
        {
            return new FileLogger(env);
        }
        public void Dispose()
        {

        }
    }
}