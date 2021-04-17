using System.Threading.Tasks;
using PortBridge.CommandLine.Commands;
using Spectre.Console.Cli;

namespace PortBridge.CommandLine
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var commandApp = new CommandApp();

            commandApp.Configure(app =>
            {
                app.SetApplicationName("azure-relay");
                app.UseStrictParsing();
                app.PropagateExceptions();

                app.AddCommand<ServerCommand>("server");
                app.AddCommand<ClientCommand>("client");
            });

            return await commandApp.RunAsync(args);
        }
    }
}
