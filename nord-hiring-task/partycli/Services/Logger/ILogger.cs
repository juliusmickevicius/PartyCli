using partycli.Domain;
using System.Collections.Generic;

namespace partycli.Services.Logger
{
    public interface ILogger
    {
        List<LogModel> GetLoggedMessage(string action);
    }
}
