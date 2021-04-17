using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine
{
    [Description("Forwards a port on the local machine (or reachable machine on the local network) via Azure Relay")]
    public sealed class ServerCommand : Command<ServerCommandSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ServerCommandSettings settings)
        {
            PortBridgeServiceForwarderHost host = new PortBridgeServiceForwarderHost();

            host.Forwarders.Add(
                new ServiceConnectionForwarder(
                    settings.ServiceNamespace,
                    settings.AccessRuleName,
                    settings.AccessRuleKey,
                    "localhost",
                    settings.ConnectionName,
                    settings.Port.ToString(),
                    string.Empty));

            host.Open();

            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();

            host.Close();

            return 0;
        }
    }
}
