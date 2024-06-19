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

        public async Task<State> ProcessArgumentsAsync(ArgumentOptions argument)
        {
            var currentState = State.none;
            var serverList = string.Empty;

            if (currentState is State.none &&
                argument.PrimaryArgument is ParentArgument.server_list)
            {
                currentState = State.server_list;

                serverList = !AreAnySubTypesSelected(argument)
                    ? await _serverService.GetAllServerByCountryListAsync() 
                    : await ProcessSubArgumentsAsync(argument);
            }

            if (string.IsNullOrWhiteSpace(serverList))
            {
                if (argument.IsLocal)
                {
                    Console.WriteLine("Error: There are no server data in local storage");
                }
                else
                {
                    Console.WriteLine("Error: No server data available");
                }

                return State.none;
            }
            else
            {
                SaveLogAndServers(serverList);
                _messageDisplayService.DisplayLine(serverList);
            }

            return currentState;
        }

        private async Task<string> ProcessSubArgumentsAsync(ArgumentOptions argumentOptions)
        {
            string serverList = string.Empty;

            if (argumentOptions.IsLocal)
            {
                serverList = _settingsRepository.GetServerListData();
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

        private void SaveLogAndServers(string serverList)
        {
            var currentLog = new LogModel
            {
                Action = "Saved new server list: " + serverList,
                Time = DateTime.Now
            };

            _settingsRepository.Upsert("serverlist", serverList);
            _settingsRepository.UpsertLog(currentLog);
        }

        private bool AreAnySubTypesSelected(ArgumentOptions options)
        {
            return options.IsFrance || options.IsTcp || options.IsLocal;
        }
    }
}
