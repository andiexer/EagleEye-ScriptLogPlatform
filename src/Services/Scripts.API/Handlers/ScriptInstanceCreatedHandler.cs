using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.BuilidingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace EESLP.Services.Scripts.API.Handlers
{
    public class ScriptInstanceCreatedHandler : IEventHandler<ScriptInstanceCreated>
    {
        private readonly ILogger<ScriptInstanceCreatedHandler> _logger;

        public ScriptInstanceCreatedHandler(ILogger<ScriptInstanceCreatedHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ScriptInstanceCreated @event)
        {
            _logger.LogWarning($"message received my friend: {@event.ScriptInstanceId} - {@event.Description}");
            await Task.CompletedTask;
        }
    }
}
