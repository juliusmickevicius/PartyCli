using partycli.Domain.Enums;
using System;
using System.Threading.Tasks;
using partycli.Services.Server;
using partycli.Domain;
using partycli.Options;
using partycli.Infrastructure.Repository.Settings;
using partycli.Services.MessageDisplay;

namespace partycli.Services.ArgumentHandlerService
{
    public class ArgumentHandlerService : IArgumentHandlerService
    {
        private readonly IServersHttpService _serverService;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMessageDisplayService _messageDisplayService;

        public ArgumentHandlerService(IServersHttpService serverService, ISettingsRepository settingsRepository, IMessageDisplayService messageDisplayService)
        {
            _serverService = serverService;
            _settingsRepository = settingsRepository;
            _messageDisplayService = messageDisplayService;
        }

        public async Task<State> ProcessArgumentsAsync(ArgumentOptions args)
        {
            var currentState = State.none;
            return await ProcessPrimaryArgument(currentState, args);
        }

        private async Task<State> ProcessPrimaryArgument(State currentState, ArgumentOptions argument)
        {
            string serverList = string.Empty;

            //Since the only available primary argument at this time is server_list, there is no need to cater further
            if (currentState is State.none &&
                argument.PrimaryArgument == ParentArgument.server_list)
            {
                currentState = State.server_list;

                if (!AreAnySubTypesSelected(argument))
                {
                    serverList = await _serverService.GetAllServerByCountryListAsync();          
                }
                else
                {
                    serverList = await ProcessSecondaryArgument(argument);
                }
            }

            //We dont need to log and display server list if its taken from local data storage
            if(!string.IsNullOrWhiteSpace(serverList) || !argument.IsLocal)
                SaveLogAndDisplayServerList(serverList);

            return currentState;
        }

        private async Task<string> ProcessSecondaryArgument(ArgumentOptions argumentOptions)
        {
            string serverList = string.Empty;

            if (argumentOptions.IsLocal)
            {
                GetAndDisplayServers();
            }
            else if (argumentOptions.IsFrance)
            {
                serverList = await _serverService.GetAllServerByCountryListAsync((int)Country.France);
            }
            else if (argumentOptions.IsTcp)
            {
                serverList = await _serverService.GetAllServerByProtocolListAsync((int)Protocol.Tcp);
            }

            return serverList;
        }

        private void GetAndDisplayServers()
        {
            var serverData = _settingsRepository.GetServerListData();

            if (!string.IsNullOrEmpty(serverData))
            {
                _messageDisplayService.DisplayLine(serverData);
            }
            else
            {
                Console.WriteLine("Error: There are no server data in local storage");
            }
        }

        private void SaveLogAndDisplayServerList(string serverList)
        {
            _settingsRepository.Upsert("serverlist", serverList);

            var currentLog = new LogModel
            {
                Action = "Saved new server list: " + serverList,
                Time = DateTime.Now
            };

            _settingsRepository.UpsertLog(currentLog);
            _messageDisplayService.DisplayLine(serverList);
        }

        private bool AreAnySubTypesSelected(ArgumentOptions options)
        {
            return options.IsFrance || options.IsTcp || options.IsLocal;
        }
    }
}
