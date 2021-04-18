// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PortBridge
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Relay;

    public class ServiceConnectionForwarder
    {
        readonly List<int> allowedPorts;
        readonly Uri endpointVia;
        readonly bool noPortConstraints;
        readonly HybridConnectionListener relayListener;
        readonly string targetHost;
        readonly TokenProvider tokenProvider;

        public ServiceConnectionForwarder(
            string serviceNamespace,
            string ruleName,
            string ruleKey,
            string targetHost,
            string targetHostAlias,
            string allowedPortsString)
        {
            this.targetHost = targetHost;
            noPortConstraints = false;
            allowedPorts = new List<int>();

            allowedPortsString = allowedPortsString.Trim();
            if (allowedPortsString == "*")
            {
                noPortConstraints = true;
            }
            else
            {
                noPortConstraints = false;
                string[] portList = allowedPortsString.Split(',');
                for (int i = 0; i < portList.Length; i++)
                {
                    allowedPorts.Add(int.Parse(portList[i].Trim()));
                }
            }

            endpointVia = new UriBuilder("sb", serviceNamespace, -1, targetHostAlias).Uri;

            tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ruleName, ruleKey);
            relayListener = new HybridConnectionListener(endpointVia, tokenProvider);

            relayListener.Online += (s, e) => Trace.TraceInformation("{0} Online", relayListener);
            relayListener.Connecting += (s, e) => Trace.TraceWarning("{0} Re-connecting! {1}: {2}", relayListener, relayListener.LastError.GetType(), relayListener.LastError.Message);
            relayListener.Offline += (s, e) => Trace.TraceError("{0} Offline! {1}: {2}", relayListener, relayListener.LastError.GetType(), relayListener.LastError.Message);
        }

        public async Task<bool> OpenService()
        {
            try
            {
                await relayListener.OpenAsync(CancellationToken.None);
#pragma warning disable 4014
                relayListener.AcceptConnectionAsync().ContinueWith(t => StreamAccepted(t.Result));
#pragma warning restore 4014
            }
            catch (Exception e)
            {
                Trace.TraceError("Unable to connect: {0}", e.Message);
                return false;
            }
            return true;
        }

        public async Task CloseService()
        {
            await relayListener.CloseAsync(CancellationToken.None);
        }

        void StreamAccepted(HybridConnectionStream hybridConnectionStream)
        {
            try
            {
                if (hybridConnectionStream != null)
                {
                    relayListener.AcceptConnectionAsync().ContinueWith(t => StreamAccepted(t.Result));
                    var preambleReader = new BinaryReader(hybridConnectionStream);
                    var connectionInfo = preambleReader.ReadString();
                    if (connectionInfo.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
                    {
                        int port;

                        if (!int.TryParse(connectionInfo.Substring(4), out port))
                        {
                            try
                            {
                                hybridConnectionStream.Close();
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("Error closing stream: {0}", ex.Message);
                            }
                            return;
                        }
                        bool portAllowed = noPortConstraints;
                        Trace.TraceInformation("Incoming connection for port {0}", port);
                        if (!portAllowed)
                        {
                            for (int i = 0; i < allowedPorts.Count; i++)
                            {
                                if (port == allowedPorts[i])
                                {
                                    portAllowed = true;
                                    break;
                                }
                            }
                        }
                        if (!portAllowed)
                        {
                            Trace.TraceWarning("Incoming connection for port {0} not permitted", port);
                            try
                            {
                                hybridConnectionStream.Close();
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("Error closing stream: {0}", ex.Message);
                            }
                            return;
                        }
                    }
                    else
                    {
                        Trace.TraceError("Unable to handle connection for {0}", connectionInfo);
                        hybridConnectionStream.Close();
                        return;
                    }

                    MultiplexConnectionInputPump connectionPump =
                        new MultiplexConnectionInputPump(
                            hybridConnectionStream.Read,
                            OnCreateConnection,
                            new StreamConnection(hybridConnectionStream, connectionInfo));
                    connectionPump.Run(false);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error accepting connection: {0}", ex.Message);
            }
        }

        MultiplexedConnection OnCreateConnection(int connectionId, object streamConnectionObject)
        {
            StreamConnection streamConnection = (StreamConnection) streamConnectionObject;

            if (streamConnection.ConnectionInfo.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
            {
                int port = int.Parse(streamConnection.ConnectionInfo.Substring(4));
                TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);
                tcpClient.LingerState.Enabled = true;
                tcpClient.NoDelay = true;
                tcpClient.Connect(targetHost, port);

                return new MultiplexedServiceTcpConnection(streamConnection, tcpClient, connectionId);
            }
            
            throw new InvalidOperationException();
        }
    }
}