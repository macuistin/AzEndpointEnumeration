using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Management.FrontDoor;
using Microsoft.Azure.Management.FrontDoor.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Rest.Azure;
using System;
using System.Threading.Tasks;
using CommandLine;
using System.Collections.Generic;

namespace AzEndpointEnumeration
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CliOptions>(args)
                .WithNotParsed(HandleParseError)
                .WithParsedAsync(FindApisInFrontDoorAsync);            
        }

        private static void HandleParseError(IEnumerable<CommandLine.Error> errors)
        {
            var existingColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Command line errors");
            Console.ForegroundColor = existingColour;
        }

        private static async Task FindApisInFrontDoorAsync(CliOptions options)
        {
            var azureCredentials = GetAzAuthentication();

            var frontDoors = await GetFrontDoorsAsync(options, azureCredentials);

            Console.WriteLine(frontDoors.ToJson(options));
        }

        private static async Task<IPage<FrontDoorModel>> GetFrontDoorsAsync(CliOptions options, AzureCredentials azureCredentials)
        {
            var client = new FrontDoorManagementClient(azureCredentials)
            {
                SubscriptionId = options.SubscriptionId
            };

            var frontDoors = await client.FrontDoors.ListByResourceGroupAsync(options.ResourceGroupName);
            return frontDoors;
        }

        private static AzureCredentials GetAzAuthentication()
        {
            var defaultCredential = new DefaultAzureCredential();
            var defaultToken = defaultCredential.GetToken(new TokenRequestContext(new[] { "https://management.azure.com/.default" })).Token;
            var defaultTokenCredentials = new Microsoft.Rest.TokenCredentials(defaultToken);
            var azureCredentials = new AzureCredentials(defaultTokenCredentials, defaultTokenCredentials, null, AzureEnvironment.AzureGlobalCloud);
            return azureCredentials;
        }
    }
}
