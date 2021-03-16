using CloudNative.CloudEvents;
using System;
using System.Collections.Generic;

namespace DemoCloudEventsNuget
{
    class MyExtension : ICloudEventExtension
    {

        private const string CustomAttributeName = "custom";

        public string Custom
        {
            get => allAttributes[CustomAttributeName] as string;
            set => allAttributes[CustomAttributeName] = value;
        }

        private IDictionary<string, object> allAttributes = new Dictionary<string, object>();


        //This method must be implmented for adding the attribute(s) to the CloudEvent instance
        public void Attach(CloudEvent cloudEvent)
        {

            var eventAttributes = cloudEvent.GetAttributes();
            if (allAttributes == eventAttributes)
            {
                // already done
                return;
            }

            foreach (var attr in allAttributes)
            {
                if (attr.Value != null)
                {
                    eventAttributes[attr.Key] = attr.Value;
                }
            }
            allAttributes = eventAttributes; //This keep reference of the attributtes values when decoding  
        }

        public Type GetAttributeType(string name)
        {
            switch (name)
            {
                case CustomAttributeName:
                    return typeof(string);
            }
            return null;
        }

        public bool ValidateAndNormalize(string key, ref dynamic value)
        {
            switch (key)
            {
                case CustomAttributeName:
                    if (value is string)
                        return true;

                    throw new InvalidOperationException();

            }
            return false;

        }
    }
}
