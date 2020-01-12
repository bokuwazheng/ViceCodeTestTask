using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViceCodeTestTask
{
    public class FirebaseResponseError
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class FirebaseResponseRoot
    {
        [JsonProperty("error")]
        public FirebaseResponseError Error { get; set; }
    }
}
