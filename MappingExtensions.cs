using AzEndpointEnumeration.Models;
using Microsoft.Azure.Management.FrontDoor.Models;
using Microsoft.Rest.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Endpoint = AzEndpointEnumeration.Models.Endpoint;

namespace AzEndpointEnumeration
{
    internal static class MappingExtensions
    {
        internal static string ToJson(this IPage<FrontDoorModel> frontdoors, CliOptions options)
        {
            var models = frontdoors.ToModels(options);

            return JsonSerializer.Serialize(models, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }

        internal static IEnumerable<FrontDoor> ToModels(this IPage<FrontDoorModel> frontdoors, CliOptions options)
         => frontdoors.Select(fd => fd.ToModel(options));

        internal static FrontDoor ToModel(this FrontDoorModel frontdoor, CliOptions options)
        => new()
            {
                Name = frontdoor.Name,
                Endpoints = frontdoor.FrontendEndpoints
                            .Where(e => options.WithNoFirewall || e.WebApplicationFirewallPolicyLink != null)
                            .Select(e => e.ToModel(options))
            };

        internal static Endpoint ToModel(this FrontendEndpoint endpoint, CliOptions options)
         => new()
         {
             Name = endpoint.Name,
             HostName = endpoint.HostName,
             Type = endpoint.ToEndpointType(options)
         };

        internal static Models.EndpointType ToEndpointType(this FrontendEndpoint endpoint, CliOptions options)
        {
            if (string.IsNullOrWhiteSpace(endpoint.Name))
                return Models.EndpointType.Unknown;

            if (!string.IsNullOrWhiteSpace(options.ApiRegex)
                && Regex.IsMatch(endpoint.Name, options.ApiRegex))
                return Models.EndpointType.API;

            if (!string.IsNullOrWhiteSpace(options.UiRegex)
                && Regex.IsMatch(endpoint.Name, options.UiRegex))
                return Models.EndpointType.UI;

            return Models.EndpointType.Unknown;
        }
    }
}
