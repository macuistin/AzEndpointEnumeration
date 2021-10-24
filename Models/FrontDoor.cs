using System.Collections.Generic;

namespace AzEndpointEnumeration.Models
{
    internal class FrontDoor
    {
        public string Name { get; set; }
        public IEnumerable<Endpoint> Endpoints { get; set; }
    }
}
