using partycli.Domain.Enums;
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

            var currentState = await _commandProcessorService.ProcessArgumentsAsync(args);


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
