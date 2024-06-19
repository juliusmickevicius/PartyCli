using partycli.Domain.Enums;
using System;
using System.Threading.Tasks;
using partycli.Infrastructure.Repository;
using partycli.Services.Server;
using Newtonsoft.Json;
using partycli.Domain;
using System.Collections.Generic;
using partycli.Services.Logger;
using partycli.Domain.Constants;

namespace partycli.Services.ArgumentHandlerService
{
    public class ArgumentHandlerService : IArgumentHandlerService
    {
        private readonly IServersHttpService _serverService;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ILogger _logger;

        public ArgumentHandlerService(IServersHttpService serverService, ISettingsRepository settingsRepository, ILogger logger)
        {
            _serverService = serverService;
            _settingsRepository = settingsRepository;
            _logger = logger;
        }

        public async Task<State> ProcessArgumentsAsync(string[] args)
        {
            var currentState = State.none;
            return await ProcessPrimaryArgument(currentState, args);
        }

        private async Task<State> ProcessPrimaryArgument(State currentState, string[] argument)
        {
            string serverList = string.Empty;

            if (currentState is State.none &&
                argument[0] == ArgumentConstants.PRIMARY_ARG_SERVER_LIST)
            {
                currentState = State.server_list;

                if (argument.Length is 1)
                {
                    serverList = await _serverService.GetAllServerByCountryListAsync();
                    LogAndDisplayServerList(serverList);
                }
                else
                {
                    await ProcessSecondaryArgument(argument);
                }
            }

            return currentState;
        }

        private async Task ProcessSecondaryArgument(string[] args)
        {

            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == ArgumentConstants.SECONDARY_ARG_LOCAL)
                {
                    GetAndDisplayServers();
                    break;
                }
                else if (args[i] == ArgumentConstants.SECONDARY_ARG_CNTR_FRANCE)
                {
                    var serverList = await _serverService.GetAllServerByCountryListAsync((int)Protocol.Tcp);
                    LogAndDisplayServerList(serverList);
                    break;
                }
                else if (args[i] == ArgumentConstants.SECONDARY_ARG_PRTCL_TCP)
                {
                    var serverList = await _serverService.GetAllServerByProtocolListAsync((int)Protocol.Tcp);
                    LogAndDisplayServerList(serverList);
                    break;
                }
            }
        }

        private void GetAndDisplayServers()
        {
            var serverData = _settingsRepository.GetServerListData();

            if (!string.IsNullOrEmpty(serverData))
            {
                displayList(serverData);
            }
            else
            {
                Console.WriteLine("Error: There are no server data in local storage");
            }
        }

        private void LogAndDisplayServerList(string serverList)
        {
            _settingsRepository.InsertValue("serverlist", serverList);
            SaveLog("Saved new server list: " + serverList);
            displayList(serverList);
        }

        static void displayList(string serverListString)
        {
            var serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);
            Console.WriteLine("Server list: ");
            for (var index = 0; index < serverlist.Count; index++)
            {
                Console.WriteLine("Name: " + serverlist[index].Name);
            }
            Console.WriteLine("Total servers: " + serverlist.Count);
        }

        private void SaveLog(string action)
        {
            var currentLog = _logger.GetLoggedMessage(action);
            _settingsRepository.InsertValue("log", JsonConvert.SerializeObject(currentLog));
        }
    }
}
