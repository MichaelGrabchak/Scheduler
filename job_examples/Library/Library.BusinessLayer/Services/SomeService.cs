using System;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Scheduler.Logging;

namespace Library.BusinessLayer.Services
{
    public class SomeService
    {
        private readonly ILogger _logger;

        public SomeService(ILoggerProvider loggerProvider)
        {
            _logger = loggerProvider.GetLogger("CustomJobLogger");
        }

        public string DoSomething()
        {
            _logger.Debug("Start DoSomething method");
            var test = new JTokenWriter();
            var test2= test.GetType().GetProperty("CurrentToken");

            var complexObject = new
            {
                Name = "ComplexObject",
                ExpiryDate = new DateTime(2017, 7, 7),
                AttributeNames = new[] { "attr1", "attr2", "attr3" },
                IsCurrentPropertyExist = (test2 != null)
            };

            // emulate execution
            Thread.Sleep(10000);

            _logger.Debug("Finish DoSomething method");

            return JsonConvert.SerializeObject(complexObject);
        }
    }
}
