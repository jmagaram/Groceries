using System.Threading.Tasks;
using static Models.DtoTypes;

#nullable enable

namespace WebApp.Services {
    public interface ICosmosConnector {
        Task<Changes> PullIncrementalAsync(string familyId, int after, int? before);
        Task<Changes> PullEverythingAsync(string familyId);
        Task<Changes> PushAsync(string familyId, Changes changes);
    }
}
