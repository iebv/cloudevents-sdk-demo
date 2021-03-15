using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using Newtonsoft.Json.Linq;
using System;
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


            //--DECODING--//
            var sampleJson = @"
               {
                   'specversion' : '1.0',
                   'type' : 'com.github.pull.create',
                   'id' : 'A234-1234-1234',
                   'source' : 'event-source',
                   'custom' : 'value-decoded'
               }";

            var jsonFormatter = new JsonEventFormatter();

            JObject obj = JObject.Parse(sampleJson);
            
            var myExt2 = new MyExtension();
     
            var cloudEvent2 = jsonFormatter.DecodeJObject(obj, new[] { myExt2 });

            Console.WriteLine($"The value of the custom attr is: {cloudEvent2.Extension<MyExtension>().Custom}");

        }
    }
}
