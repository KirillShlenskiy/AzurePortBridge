# Azure Port Bridge (fork)

This is a gutted version of one of the samples from the [Azure Relay repository](https://github.com/azure/azure-relay), PortBridge.

The aim was to provide an easy-to-use command line interface which makes it quick and easy to forward a local port via Azure Relay service's Hybrid Connection.

The work inside the PortBridge project is not my own. I've kept the tweaks to a minimum, only changing what was necessary to get PortBridge running on .NET Core.

## Server example

    azure-port-bridge server --service-namespace 'xxx.servicebus.windows.net' --access-rule-name 'RootManageSharedAccessKey' --access-rule-key '<RootManageSharedAccessKey_secret>' --connection-name 'my-hybrid-mssql-connection' --port 1433

## Client example

    azure-port-bridge client --service-namespace 'xxx.servicebus.windows.net' --access-rule-name 'RootManageSharedAccessKey' --access-rule-key '<RootManageSharedAccessKey_secret>' --connection-name 'my-hybrid-mssql-connection' --local-port 9999 --remote-port 1433
