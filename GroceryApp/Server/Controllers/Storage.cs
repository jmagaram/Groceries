using GroceryApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Models.ServiceTypes;
using static Models.DtoTypes;
using Models;
using GroceryApp.Common;

namespace GroceryApp.Server.Controllers
{

    // Storage interface should pass a timeout (or no token at all)

    //type ICosmosConnector =
    //abstract CreateDatabaseAsync: unit -> Task
    //abstract DeleteDatabaseAsync: unit -> Task
    //abstract PushAsync : DtoTypes.Changes -> token:CancellationToken -> Task<DtoTypes.Changes>
    //abstract PullSinceAsync: lastSync:int -> earlierThan: int option -> token:CancellationToken -> Task<DtoTypes.Changes>
    //abstract PullEverythingAsync: token:CancellationToken -> Task<DtoTypes.Changes>

    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ICosmosConnector _connector;

        public StorageController(ICosmosConnector connector)
        {
            _connector = connector;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetEverything()
        {
            var changes = await _connector.PullEverythingAsync();
            return changes;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Changes> GetIncremental(int after, int? before)
        {
            var changes = await _connector.PullSinceAsync(after, before.ToFSharpOption());
            return changes;
        }
    }
}
