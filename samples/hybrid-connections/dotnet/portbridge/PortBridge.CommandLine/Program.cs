using Spectre.Console.Cli;

namespace PortBridge.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            var commandApp = new CommandApp();

            commandApp.Configure(app =>
            {
                app.SetApplicationName("PortBridgeClientAgent");
                app.UseStrictParsing();
                app.PropagateExceptions();

                app.AddCommand<ServerCommand>("server");
                app.AddCommand<ClientCommand>("client");
            });

            return commandApp.Run(args);
        }
    }
}
