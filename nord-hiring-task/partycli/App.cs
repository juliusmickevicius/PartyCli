using CommandLine;
using partycli.Domain.Enums;
using partycli.Options;
using partycli.Services.ArgumentHandlerService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace partycli
{
    public class App
    {
        IArgumentHandlerService _commandProcessorService;

        public App(IArgumentHandlerService commandProcessorService)
        {
            _commandProcessorService = commandProcessorService;
        }

        public async Task Excecute(string[] args) 
        {
            var currentState = State.none;

            var parser = await Parser.Default.ParseArguments<ArgumentOptions>(args)
            .WithParsedAsync(async o =>
            {
                currentState = await _commandProcessorService.ProcessArgumentsAsync(o);
            });

            parser.WithNotParsed((er) => 
            {
                Console.WriteLine($"Cannot parse message due to error: {er.FirstOrDefault()}");
                Console.WriteLine();
            });

            if (currentState == State.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }

            Console.Read();
        }
    }
}
