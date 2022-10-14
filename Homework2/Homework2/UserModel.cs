using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework
{
    public class UserModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photoUrls")]
        public string[] PhotoUrls { get; set; }

        [JsonProperty("tags")]
        //public Category[] Tags { get; set; }
        public List<Category> Tags { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
