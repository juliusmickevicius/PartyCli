using partycli.Domain;
using System;

namespace partycli.Utilities
{
    public static class Logging
    {
        public static LogModel GetCurrentLog(string action)
        {
            return new LogModel()
            {
                Action = action,
                Time = DateTime.Now
            };
        }
    }
}
