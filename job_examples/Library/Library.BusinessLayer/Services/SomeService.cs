using System;
using System.Threading;

using Newtonsoft.Json;

namespace Library.BusinessLayer.Services
{
    public class SomeService
    {
        public string DoSomething()
        {

            var complexObject = new
            {
                Name = "ComplexObject",
                ExpiryDate = new DateTime(2017, 7, 7),
                AttributeNames = new[] { "attr1", "attr2", "attr3" }
            };

            // emulate execution
            Thread.Sleep(10000);

            return JsonConvert.SerializeObject(complexObject);
        }
    }
}
