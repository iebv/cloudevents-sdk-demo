using CloudNative.CloudEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCloudEventsNuget
{
    class MyExtension : ICloudEventExtension
    {

        private const string CustomAttributeName = "custom";

        private string _custom;
        public string Custom
        {
            get => _custom;
            set
            {
                _custom = value;
                AllAttributes.Add(CustomAttributeName, _custom);
            }
        }

        private Dictionary<string, object> AllAttributes { get; }

        public MyExtension()
        {
            AllAttributes = new Dictionary<string, object>();
        }
        

        //This methos must be implmented for adding the attribute(s) to the CloudEvent instance
        public void Attach(CloudEvent cloudEvent)
        {
            foreach(var key in AllAttributes.Keys)
                cloudEvent.GetAttributes().Add(key, AllAttributes[key]);
        }

        public Type GetAttributeType(string name)
        {
            AllAttributes.TryGetValue(name, out object value);
            return value.GetType();
        }

        public bool ValidateAndNormalize(string key, ref dynamic value)
        {
            if (AllAttributes.ContainsKey(key))
            {
                if (AllAttributes.TryGetValue(key, out object val))
                    return val == value;
            }
            return false;
               
        }
    }
}
