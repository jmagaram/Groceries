using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceriesWasmApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Models.DtoTypes;

namespace GroceriesWasmApp.Server.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase {
        private readonly ICosmosConnector _connector;

        public StorageController(ICosmosConnector connector) {
            this._connector = connector;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetEverything(string familyId) {
            var changes = await _connector.PullEverythingAsync(familyId);
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetIncremental(string familyId, int after, int? before) {
            var changes = await _connector.PullIncrementalAsync(familyId, after, before);
            return changes;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Changes> Push(string familyId, Changes changes) {
            var result = await _connector.PushAsync(familyId, changes);
            return result;
        }
    }
}
