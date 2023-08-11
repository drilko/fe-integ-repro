using FastEndpoints;

namespace FE.IntegrationTests.Repro.Infrastructure.Events;

public class EntityDeletedEvent<TEntity> : IEvent
    where TEntity : Entity
{
    public TEntity Entity { get; private set; }

    public EntityDeletedEvent(TEntity entity)
    {
        Entity = entity;
    }
}
