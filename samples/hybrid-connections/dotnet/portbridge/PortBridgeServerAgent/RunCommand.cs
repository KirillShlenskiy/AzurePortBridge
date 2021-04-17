using PortBridge;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PortBridgeServerAgent
{
    public sealed class RunCommand : Command<RunSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] RunSettings settings)
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
