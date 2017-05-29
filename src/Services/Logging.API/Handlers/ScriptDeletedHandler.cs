using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.Services.Logging.API.Services;
using Microsoft.Extensions.Logging;

namespace EESLP.Services.Logging.API.Handlers
{
    public class ScriptDeletedHandler : IEventHandler<ScriptDeleted>
    {
        private readonly ILogger<ScriptDeleted> _logger;
        private readonly IScriptInstanceService _scriptInstanceService;

        public ScriptDeletedHandler(ILogger<ScriptDeleted> logger, IScriptInstanceService scriptInstanceService)
        {
            _logger = logger;
            _scriptInstanceService = scriptInstanceService;
        }

        public async Task HandleAsync(ScriptDeleted @event)
        {
            _logger.LogDebug($"script deleted event received. delete all scriptinstances with id : {@event.Id}");
            _scriptInstanceService.DeleteByScriptId(@event.Id);
            _logger.LogDebug($"successfully deleted all scriptinstances and logs for script with id {@event.Id}");
            await Task.CompletedTask;
        }
    }
}
