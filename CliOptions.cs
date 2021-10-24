using CommandLine;

namespace AzEndpointEnumeration
{
    public class CliOptions
    {
        [Option('s', "subscription", Required = true, HelpText = "The subscription credentials which uniquely identify the Microsoft Azure subscription.")]
        public string SubscriptionId { get; set; }

        [Option('r', "resourceGroup", Required = true, HelpText = "Name of the Resource group within the Azure subscription.")]
        public string ResourceGroupName { get; set; }

        [Option('a', "apiRegex", Required = false, HelpText = "Regex used to identify APIs, based on endpoint name.")]
        public string ApiRegex { get; set; } = ".*-api";

        [Option('u', "uiRegex", Required = false, HelpText = "Regex used to identify APIs, based on endpoint name.")]
        public string UiRegex { get; set; } = ".*-ui";

        [Option('f', "WithNoFirewall", Required = false, HelpText = "Includes endpoints with no firewall.")]
        public bool WithNoFirewall { get; set; }
    }
}
