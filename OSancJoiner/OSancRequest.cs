using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSancJoiner
{
    public class OSancRequest
    {
        [JsonProperty("access_token")]
        public string accessToken;
        [JsonProperty("raid_id")]
        public int raidId;
        [JsonProperty("react_id")]
        public int reactId;
    }
}
