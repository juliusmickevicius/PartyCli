using CommandLine;
using partycli.Domain.Enums;
using partycli.Options;
using partycli.Services.ArgumentHandlerService;
using System;
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

            await Parser.Default.ParseArguments<ArgumentOptions>(args)
                .WithParsedAsync(async o =>
                {
                    currentState = await _commandProcessorService.ProcessArgumentsAsync(o);
                });

            if (currentState == State.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }

            Console.Read();

            //this code was not documented to be called ¯\_(ツ)_/¯
            //        if (arg == "config")
            //        {
            //            currentState = States.config;
            //        }
            //    }
            //    else if (currentState == States.config)
            //    {
            //        if (name == null)
            //        {
            //            name = arg;
            //        }
            //        else
            //        {
            //            _settingsRepository.InsertValue(proccessName(name), arg);
            //            Console.WriteLine("Changed " + proccessName(name) + " to " + arg);

            //            log("Changed " + proccessName(name) + " to " + arg);
            //            name = null;
            //        }
            //    }

        }
    }
}
