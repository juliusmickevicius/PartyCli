using partycli.Domain.Enums;
using System.Threading.Tasks;

namespace partycli.Services.ArgumentHandlerService
{
    public interface IArgumentHandlerService
    {
        Task<State> ProcessArgumentsAsync(string[] args);
    }
}
