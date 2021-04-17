using System.ComponentModel;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine
{
    public sealed class ServerCommandSettings : CommandSettings
    {
        [CommandOption("-n|--service-namespace")]
        [Description("Fully qualified service namespace (i.e. xxx.servicebus.windows.net")]
        public string ServiceNamespace { get; set; }

        [CommandOption("-a|--access-rule-name")]
        [Description("Access rule name. Defaults to 'RootManageSharedAccessKey'")]
        [DefaultValue("RootManageSharedAccessKey")]
        public string AccessRuleName { get; set; }

        [CommandOption("-s|--access-rule-key")]
        [Description("Access rule secret (key)")]
        public string AccessRuleKey { get; set; }

        [CommandOption("-m|--connection-name")]
        [Description("Name of the hybrid connection/host")]
        public string ConnectionName { get; set; }

        [CommandOption("-p|--port")]
        [Description("Port to forward")]
        public int Port { get; set; }
    }
}