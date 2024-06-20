using partycli.Domain;

namespace partycli.Infrastructure.Repository.Settings
{
    public interface ISettingsRepository
    {
        void Insert(string name, string value);
        string GetServerListData();

        string GetLogData();

        void UpsertLog(LogModel log);
    }
}
