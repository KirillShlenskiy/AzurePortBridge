using PortBridge;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PortBridgeClientAgent
{
    public sealed class RunCommand : Command<RunSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] RunSettings settings)
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
