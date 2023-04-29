using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Others
{
    public class UserInfo
    {
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("preferred_username")]
        public string PreferredUsername { get; set; }
        [JsonPropertyName("sub")]
        public Guid UserId { get; set; }
        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
