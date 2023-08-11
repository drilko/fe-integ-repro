namespace FE.IntegrationTests.Repro.Infrastructure.Events;
public class EntityUpdatedEvent<TEntity> : IEvent
    where TEntity : Entity
{
    public TEntity Entity { get; private set; }

    public EntityUpdatedEvent(TEntity entity)
    {
        Entity = entity;
    }
}
