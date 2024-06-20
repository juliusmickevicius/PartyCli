using Newtonsoft.Json;
using partycli.Domain;
using System.Collections.Generic;
using System;

namespace partycli.Services.MessageDisplay
{
    public class MessageDisplayService : IMessageDisplayService
    {
        public void DisplayServerList(string serverListString)
        {
            var serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);

            Console.WriteLine("Server list: ");
            for (var index = 0; index < serverlist.Count; index++)
            {
                Console.WriteLine("Name: " + serverlist[index].Name);
            }

            Console.WriteLine("Total servers: " + serverlist.Count);
        }
    }
}
