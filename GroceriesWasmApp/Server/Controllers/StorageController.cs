using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GroceriesWasmApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using static Models.DtoTypes;

namespace GroceriesWasmApp.Server.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase {
        private readonly ICosmosConnector _connector;
        static readonly string[] scopeRequiredByApi = new string[] { "API.Access" };

        public StorageController(ICosmosConnector connector) {
            this._connector = connector;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetEverything(string familyId) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var changes = await _connector.PullEverythingAsync(familyId);
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetIncremental(string familyId, int after, int? before) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var changes = await _connector.PullIncrementalAsync(familyId, after, before);
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Administrators")]
        public string AdminOnly() {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return $"Hello {DateTimeOffset.Now}";
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Changes> Push(string familyId, Changes changes) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var result = await _connector.PushAsync(familyId, changes);
            return result;
        }
    }
}
