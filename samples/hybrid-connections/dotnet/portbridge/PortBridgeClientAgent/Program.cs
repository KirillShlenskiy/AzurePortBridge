// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PortBridgeClientAgent
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using PortBridge;

    class Program
    {
        static int fromPort = -1;
        static int toPort = -1;
        static string serviceNamespace;
        static string accessRuleName;
        static string accessRuleKey;
        static string cmdlineTargetHost;
        
        static void Main(string[] args)
        {
            accessRuleKey = args[0];

            PortBridgeClientForwarderHost host = new PortBridgeClientForwarderHost();
            List<IPRange> firewallRules = new List<IPRange>();
            firewallRules.Add(new IPRange(IPAddress.Any, IPAddress.Broadcast));
            host.Forwarders.Add(
                new TcpClientConnectionForwarder(
                    serviceNamespace,
                    accessRuleName,
                    accessRuleKey,
                    cmdlineTargetHost,
                    fromPort,
                    toPort,
                    null,
                    firewallRules));

            host.Open();
            Console.WriteLine("Press [ENTER] to exit.");
            Console.ReadLine();
            host.Close();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Arguments (all arguments are required):");
            Console.WriteLine("\t-n <namespace> Service Namespace");
            Console.WriteLine("\t-s <key> Issuer Secret (Key)");
            Console.WriteLine("\t-m <machine> mapped host name of the machine running the PortBridge service");
            Console.WriteLine("\t-l <port> Local TCP port number to map from");
            Console.WriteLine("\t-r <port> Remote TCP port number to map to");
        }

        static void PrintLogo()
        {
            Console.WriteLine("Port Bridge Agent\n(c) Microsoft Corporation\n\n");
        }

        static bool ParseCommandLine(string[] args)
        {
            try
            {
                char lastOpt = default(char);

                foreach (var arg in args)
                {
                    if ((arg[0] == '-' || arg[0] == '/'))
                    {
                        if (lastOpt != default(char) || arg.Length != 2)
                        {
                            return false;
                        }
                        lastOpt = arg[1];
                        switch (lastOpt)
                        {
                            case 'c':
                            case 'C':
                                lastOpt = default(char);
                                break;
                        }
                        continue;
                    }

                    switch (lastOpt)
                    {
                        case 'N':
                        case 'n':
                            serviceNamespace = arg;
                            lastOpt = default(char);
                            break;
                        case 'S':
                        case 's':
                            accessRuleKey = arg;
                            lastOpt = default(char);
                            break;
                        case 'M':
                        case 'm':
                            cmdlineTargetHost = arg;
                            lastOpt = default(char);
                            break;
                        case 'L':
                        case 'l':
                            fromPort = int.Parse(arg);
                            lastOpt = default(char);
                            break;
                        case 'R':
                        case 'r':
                            toPort = int.Parse(arg);
                            lastOpt = default(char);
                            break;
                    }
                }

                if (lastOpt != default(char))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}