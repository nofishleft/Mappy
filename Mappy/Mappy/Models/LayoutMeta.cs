using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Mappy.Models
{
    [JsonObject]
    public class LayoutMeta
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Author { get; set; }

        [JsonProperty]
        public string Map { get; set; }
    }
}
