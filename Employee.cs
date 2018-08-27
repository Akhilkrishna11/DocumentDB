using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace mydocumentdbdemo
{
    class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }
        public String Name { get; set; }
        public int Age { get; set; }

    }
}
