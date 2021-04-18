using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine.Commands
{
    [Description("Exposes a remote port forwarded via Azure Relay on the local machine")]
    public sealed class ClientCommand : Command<ClientCommandSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ClientCommandSettings settings)
        {
            var firewallRules = new[]
            {
                new IPRange(IPAddress.Any, IPAddress.Broadcast)
            };

            using var client = new TcpClientConnectionForwarder(
                settings.ServiceNamespace,
                settings.AccessRuleName,
                settings.AccessRuleKey,
                settings.ConnectionName,
                settings.LocalPort,
                settings.RemotePort,
                null,
                firewallRules);

            client.Open();

            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();

            client.Close();

            return 0;
        }
    }
}
