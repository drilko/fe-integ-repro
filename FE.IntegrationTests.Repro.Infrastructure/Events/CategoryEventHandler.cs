using Microsoft.Extensions.Logging;

namespace FE.IntegrationTests.Repro.Infrastructure.Events
{
    public class CategoryEventHandler :
        IEventHandler<EntityUpdatedEvent<Category>>,
        IEventHandler<EntityInsertedEvent<Category>>,
        IEventHandler<EntityDeletedEvent<Category>>
    {
        private readonly ILogger<CategoryEventHandler> _logger;

        public CategoryEventHandler(ILogger<CategoryEventHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleAsync(EntityUpdatedEvent<Category> eventModel, CancellationToken ct)
        {
            _logger.LogInformation("Handling entity updated event");
            return Task.CompletedTask;
        }

        public Task HandleAsync(EntityInsertedEvent<Category> eventModel, CancellationToken ct)
        {
            _logger.LogInformation("Handling entity inserted event");
            return Task.CompletedTask;
        }

        public Task HandleAsync(EntityDeletedEvent<Category> eventModel, CancellationToken ct)
        {
            _logger.LogInformation("Handling entity deleted event");
            return Task.CompletedTask;
        }
    }
}
