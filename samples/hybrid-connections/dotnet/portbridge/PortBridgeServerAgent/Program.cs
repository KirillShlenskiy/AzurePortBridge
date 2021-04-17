using Spectre.Console.Cli;

namespace PortBridgeServerAgent
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "--help" };
            }

            var commandApp = new CommandApp();

            commandApp.SetDefaultCommand<RunCommand>();

            commandApp.Configure(app =>
            {
                app.SetApplicationName("PortBridgeServerAgent");
                app.UseStrictParsing();
                app.PropagateExceptions();
            });

            return commandApp.Run(args);
        }
    }
}