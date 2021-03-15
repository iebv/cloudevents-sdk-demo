using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace DemoCloudEventsNuget
{
    class Program
    {
        static void Main(string[] args)
        {

            //Built in extension
            var ext = new DistributedTracingExtension()
            {
                TraceParent = "value"
            };

            //Custom extension
            var myExt = new MyExtension()
            {
                Custom = "value"
            };

            ICloudEventExtension[] extensions = { ext, myExt };

            //Constructor
            var cloudEvent = new CloudEvent(
                CloudEventsSpecVersion.V1_0, //Specification
                "test-type", //Type
                new Uri("https://github.com/cloudevents/spec/pull/123"), //Source
                "test-subject", //Subject
                Guid.NewGuid().ToString(), //Id
                DateTime.Now, //time
                extensions
            )
            { 
                DataContentType = new ContentType("application/json"),
                Data = new { Name = "Ivan Ballesteros", Company = "S4N", Age = 29}
            };

            //Accessing custom extension via method
            Console.WriteLine($"The value of the custom attr is: {cloudEvent.Extension<MyExtension>().Custom}");

            //Get content
            var content = new CloudEventContent(cloudEvent,
                                     ContentMode.Structured,
                                     new JsonEventFormatter());

            //Print result as JSON
            Console.WriteLine(content.ReadAsStringAsync().Result);
            

        }
    }
}
