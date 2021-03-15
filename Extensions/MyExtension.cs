using CloudNative.CloudEvents;
using System;
using System.Collections.Generic;

namespace DemoCloudEventsNuget
{
    class MyExtension : ICloudEventExtension
    {

        private const string CustomAttributeName = "custom";

        private string _custom;
        public string Custom
        {
            get
            {
                if (_custom == null && AllAttributes[CustomAttributeName] != null)
                    _custom = AllAttributes[CustomAttributeName].ToString();
                return _custom;
            }
            set
            {
                _custom = value;
                AllAttributes[CustomAttributeName] = _custom;
            }
        }

        private Dictionary<string, object> AllAttributes { get; }

        public MyExtension()
        {
            AllAttributes = new Dictionary<string, object>();
            AllAttributes.Add(CustomAttributeName, null);
        }
        

        //This method must be implmented for adding the attribute(s) to the CloudEvent instance
        public void Attach(CloudEvent cloudEvent)
        {
            foreach (var key in AllAttributes.Keys)
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
                {
                    //This validation is needed when decoding the value of the attribute
                    if (val == null && value != null) 
                    {
                        val = value;
                        AllAttributes[key] = val;
                    }
                    return val == value;
                }
            }
            return false;
               
        }
    }
}
