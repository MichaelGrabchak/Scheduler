using System;
using System.Threading;

using Scheduler.Core.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.BusinessLayer.Services
{
    public class SomeService
    {
        private readonly ISchedulerLogger Logger = SchedulerLogManager.GetJobLogger("CustomJobLogger");

        public string DoSomething()
        {
            Logger.Debug("Start DoSomething method");
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

            Logger.Debug("Finish DoSomething method");

            return JsonConvert.SerializeObject(complexObject);
        }
    }
}
