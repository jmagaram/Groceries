using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using GroceriesWasmApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Models;
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

        // Security; can't get everything if you are not a member of the family
        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetEverything(string familyId) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await VeryCurrentUserIsMemberOfFamily(familyId);
            var changes = await _connector.PullEverythingAsync(familyId);
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetIncremental(string familyId, int after, int? before) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await VeryCurrentUserIsMemberOfFamily(familyId);
            var changes = await _connector.PullIncrementalAsync(familyId, after, before);
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Administrators")]
        public string AdminOnly() {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            //https://graph.microsoft.com/v1.0/users
            return $"Hello {DateTimeOffset.Now}";
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Changes> Push(string familyId, Changes changes) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await VeryCurrentUserIsMemberOfFamily(familyId);
            var result = await _connector.PushAsync(familyId, changes);
            return result;
        }

        static string CurrentUserEmail(HttpContext context) {
            string emailString = context.User.Claims.First(i => i.Type == "emails").Value;
            return EmailAddressModule.normalizer(emailString);
        }

        async Task VeryCurrentUserIsMemberOfFamily(string familyId) {
            if (false == (await MemberOf()).Any(i => i.CustomerId == familyId)) {
                throw new InvalidOperationException("The current user is not a member of the family.");
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Document<Family>> UpsertFamily(Document<Family> family) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            string userEmailString = CurrentUserEmail(HttpContext);
            var userEmail = EmailAddressModule.tryParse(userEmailString).Value;
            var tryDeserialize = Dto.deserializeFamily(family);
            if (tryDeserialize.IsError) {
                throw new InvalidOperationException($"The family document is not formatted properly: {tryDeserialize.ErrorValue}");
            }
            if (!tryDeserialize.ResultValue.Members.Contains(userEmail)) {
                throw new InvalidOperationException("The current user must be a member of the family.");
            }
            // Could overwrite someone else's shopping list if you guessed the ID and
            // etag correctly
            var result = await _connector.UpsertFamily(family);
            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Document<Family>[]> MemberOf() {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var userEmail = CurrentUserEmail(HttpContext);
            var changes = await _connector.MemberOf(userEmail);
            return changes;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task DeleteFamily(string familyId) {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await VeryCurrentUserIsMemberOfFamily(familyId);
            await _connector.DeleteFamily(familyId);
        }
    }
}
