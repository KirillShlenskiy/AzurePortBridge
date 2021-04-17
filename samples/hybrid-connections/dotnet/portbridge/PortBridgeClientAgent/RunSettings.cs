using Spectre.Console.Cli;
using System.ComponentModel;

namespace PortBridgeClientAgent
{
    public sealed class RunSettings : CommandSettings
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

        [CommandOption("-l|--local-port")]
        [Description("Local port to listen on")]
        public int LocalPort { get; set; }

        [CommandOption("-r|--remote-port")]
        [Description("Remote port to forward")]
        public int RemotePort { get; set; }
    }
}