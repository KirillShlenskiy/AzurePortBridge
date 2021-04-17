using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine
{
    [Description("Exposes a remote port forwarded via Azure Relay on the local machine")]
    public sealed class ClientCommand : Command<ClientCommandSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ClientCommandSettings settings)
        {
            PortBridgeClientForwarderHost host = new PortBridgeClientForwarderHost();
            List<IPRange> firewallRules = new List<IPRange>();
            firewallRules.Add(new IPRange(IPAddress.Any, IPAddress.Broadcast));

            host.Forwarders.Add(
                new TcpClientConnectionForwarder(
                    settings.ServiceNamespace,
                    settings.AccessRuleName,
                    settings.AccessRuleKey,
                    settings.ConnectionName,
                    settings.LocalPort,
                    settings.RemotePort,
                    null,
                    firewallRules));

            host.Open();

            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();

            host.Close();

            return 0;
        }
    }
}
