using System.Threading.Tasks;
using static Models.DtoTypes;

#nullable enable

namespace WebApp.Services {
    public interface ICosmosConnector {
        Task<Changes> PullIncrementalAsync(int after, int? before);
        Task<Changes> PullEverythingAsync();
        Task<Changes> PushAsync(Changes changes);
    }
}
