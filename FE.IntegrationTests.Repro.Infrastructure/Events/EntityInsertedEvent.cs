using FastEndpoints;

namespace FE.IntegrationTests.Repro.Infrastructure.Events;

public class EntityInsertedEvent<TEntity> : IEvent
    where TEntity : Entity
{
    public TEntity Entity { get; private set; }

    public EntityInsertedEvent(TEntity entity)
    {
        Entity = entity;
    }
}
