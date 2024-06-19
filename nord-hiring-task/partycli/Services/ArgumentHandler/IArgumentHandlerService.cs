using partycli.Domain.Enums;
using partycli.Options;
using System.Threading.Tasks;

namespace partycli.Services.ArgumentHandlerService
{
    public interface IArgumentHandlerService
    {
        Task<State> ProcessArgumentsAsync(ArgumentOptions args);
    }
}
