using System.Text.Json.Serialization;

namespace AzEndpointEnumeration.Models
{
    internal class Endpoint
    {
        public string Name { get; set; }
        public string HostName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EndpointType Type { get; set; }
    }
}
