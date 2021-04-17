namespace PortBridgeServerAgent
{
    using System;
    using PortBridge;

    class Program
    {
        static string serviceNamespace;
        static string accessRuleName;
        static string accessRuleKey;
        static string permittedPorts;

        static void Main(string[] args)
        {
            accessRuleKey = args[0];

            PortBridgeServiceForwarderHost host = new PortBridgeServiceForwarderHost();

            host.Forwarders.Add(
                new ServiceConnectionForwarder(
                    serviceNamespace,
                    accessRuleName,
                    accessRuleKey,
                    "localhost",
                    "kirkinputer",
                    permittedPorts,
                    string.Empty));

            host.Open();
            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();
            host.Close();
        }
    }
}