using System.Threading.Tasks;
using static Models.DtoTypes;

#nullable enable

namespace GroceriesWasmApp.Shared {
    public interface ICosmosConnector {
        Task<Changes> PullIncrementalAsync(string familyId, int after, int? before);
        Task<Changes> PullEverythingAsync(string familyId);
        Task<Changes> PushAsync(string familyId, Changes changes);
        Task<Document<Family>> UpsertFamily(Document<Family> family);
        Task<Document<Family>[]> MemberOf(string userEmail);
        Task DeleteFamily(string familyId);
    }
}
