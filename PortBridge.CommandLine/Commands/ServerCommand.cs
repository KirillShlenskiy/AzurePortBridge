using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine.Commands
{
    [Description("Forwards a port on the local machine (or reachable machine on the local network) via Azure Relay")]
    public sealed class ServerCommand : AsyncCommand<ServerCommandSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, ServerCommandSettings settings)
        {
            var server = new ServiceConnectionForwarder(
                settings.ServiceNamespace,
                settings.AccessRuleName,
                settings.AccessRuleKey,
                "localhost",
                settings.ConnectionName,
                settings.Port.ToString());

            await server.OpenService();

            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();

            await server.CloseService();

            return 0;
        }
    }
}
