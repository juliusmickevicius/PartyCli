using Newtonsoft.Json;
using partycli.Domain;
using System.Collections.Generic;
using System;

namespace partycli.Services.Logger
{
    internal class Logger : ILogger
    {
        public List<LogModel> GetLoggedMessage(string action)
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

            return currentLog;
        }
    }
}
