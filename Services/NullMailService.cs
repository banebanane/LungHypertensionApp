using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LungHypertensionApp.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            this.logger = logger;
        }
        public void SendMessage(string from, string to, string message)
        {
            // Log message
            logger.LogInformation(message);
        }
    }
}
