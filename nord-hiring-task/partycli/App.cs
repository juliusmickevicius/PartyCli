using Newtonsoft.Json;
using partycli.Domain;
using partycli.Domain.Enums;
using partycli.Infrastructure.Repository;
using partycli.Queries;
using partycli.Services.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace partycli
{
    public class App
    {
        IServerService _serverService;
        ISettingsRepository _settingsRepository;
        public App(IServerService serverService, ISettingsRepository settingsRepository)
        {
            _serverService = serverService;
            _settingsRepository = settingsRepository;
        }

        public async Task Excecute(string[] args) 
        {
            var currentState = States.none;
            string name = null;
            int argIndex = 1;

            foreach (string arg in args)
            {
                if (currentState == States.none)
                {
                    if (arg == "server_list")
                    {
                        currentState = States.server_list;
                        if (argIndex >= args.Count())
                        {
                            var serverList = await _serverService.GetAllServersListAsync();
                            _settingsRepository.InsertValue("serverlist", serverList);
                            log("Saved new server list: " + serverList);
                            displayList(serverList);
                        }
                    }
                    if (arg == "config")
                    {
                        currentState = States.config;
                    }
                }
                else if (currentState == States.config)
                {
                    if (name == null)
                    {
                        name = arg;
                    }
                    else
                    {
                        _settingsRepository.InsertValue(proccessName(name), arg);
                        Console.WriteLine("Changed " + proccessName(name) + " to " + arg);

                        log("Changed " + proccessName(name) + " to " + arg);
                        name = null;
                    }
                }
                else if (currentState == States.server_list)
                {
                    if (arg == "--local")
                    {
                        if (!String.IsNullOrEmpty(Properties.Settings.Default.serverlist))
                        {
                            displayList(Properties.Settings.Default.serverlist);
                        }
                        else
                        {
                            Console.WriteLine("Error: There are no server data in local storage");
                        }
                    }
                    else if (arg == "--france")
                    {
                        //france == 74
                        //albania == 2
                        //Argentina == 10
                        var query = new VpnServerQuery(null, 74, null, null, null, null);
                        var serverList = await _serverService.GetAllServerByCountryListAsync(query.CountryId.Value); //France id == 74
                        _settingsRepository.InsertValue("serverlist", serverList);
                        log("Saved new server list: " + serverList);
                        displayList(serverList);
                    }
                    else if (arg == "--TCP")
                    {
                        //UDP = 3
                        //Tcp = 5
                        //Nordlynx = 35
                        var query = new VpnServerQuery(5, null, null, null, null, null);
                        var serverList = await _serverService.GetAllServerByProtocolListAsync((int)query.Protocol.Value);
                        _settingsRepository.InsertValue("serverlist", serverList);
                        log("Saved new server list: " + serverList);
                        displayList(serverList);
                    }
                }
                argIndex = argIndex + 1;
            }

            if (currentState == States.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }
            Console.Read();
        }

        static string proccessName(string name)
        {
            name = name.Replace("-", string.Empty);
            return name;
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

        private void log(string action)
        {
            var newLog = new LogModel
            {
                Action = action,
                Time = DateTime.Now
            };
            List<LogModel> currentLog;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.log))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(Properties.Settings.Default.log);
                currentLog.Add(newLog);
            }
            else
            {
                currentLog = new List<LogModel> { newLog };
            }

            _settingsRepository.InsertValue("log", JsonConvert.SerializeObject(currentLog));
        }

        static void storeValue(string name, string value, bool writeToConsole = true)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole)
                {
                    Console.WriteLine("Changed " + name + " to " + value);
                }
            }
            catch
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }
        }
    }

    class LogModel
    {
        public string Action { get; set; }
        public DateTime Time { get; set; }
    }
}
