﻿using Newtonsoft.Json;
using partycli.Domain;
using System;
using System.Collections.Generic;

namespace partycli.Infrastructure.Repository.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private Properties.Settings _settingsContext = Properties.Settings.Default;

        public void Upsert(string name, string value)
        {
            try
            {
                _settingsContext[name] = value;
                _settingsContext.Save();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
                Console.WriteLine(ex.ToString());
            }
        }

        public string GetServerListData()
        {
            return _settingsContext.serverlist;
        }

        public void UpsertLog(LogModel log)
        {
            var currentLog = new List<LogModel>();

            var logData = _settingsContext.log;

            if (!string.IsNullOrEmpty(logData))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(logData);
            }

            currentLog.Add(log);

            Upsert("log", JsonConvert.SerializeObject(currentLog));
        }

        public string GetLogData()
        {
            return Properties.Settings.Default.log;
        }
    }
}
