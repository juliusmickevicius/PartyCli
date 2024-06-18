namespace partycli.Infrastructure.Repository
{
    public interface ISettingsRepository
    {
        void InsertValue(string name, string value);
    }
}
