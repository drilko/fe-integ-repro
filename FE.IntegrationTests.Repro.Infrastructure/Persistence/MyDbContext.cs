using Microsoft.EntityFrameworkCore;

namespace FE.IntegrationTests.Repro.Infrastructure.Persistence;

public class MyDbContext : DbContext
{
    private readonly bool _raiseEvents;

    public DbSet<Category> Categories { get; set; }
    public DbSet<Language> Languages { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options, bool raiseEvents = true)
    : base(options)
    {
        _raiseEvents = raiseEvents;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_raiseEvents)
        {
            await RaiseEntityEvents();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task RaiseEntityEvents()
    {
        foreach (var entity in ChangeTracker.Entries<Entity>().Where(entry => entry.State == EntityState.Modified).Select(x => x.Entity))
        {
            var eventType = typeof(EntityUpdatedEvent<>).MakeGenericType(entity.GetType());

            var @event = Activator.CreateInstance(eventType, entity) as IEvent;

            await @event!.PublishAsync();
        }

        foreach (var entity in ChangeTracker.Entries<Entity>().Where(entry => entry.State == EntityState.Added).Select(x => x.Entity))
        {
            var eventType = typeof(EntityInsertedEvent<>).MakeGenericType(entity.GetType());

            var @event = Activator.CreateInstance(eventType, entity) as IEvent;

            await @event!.PublishAsync();
        }

        foreach (var entity in ChangeTracker.Entries<Entity>().Where(entry => entry.State == EntityState.Deleted).Select(x => x.Entity))
        {
            var eventType = typeof(EntityDeletedEvent<>).MakeGenericType(entity.GetType());

            var @event = Activator.CreateInstance(eventType, entity) as IEvent;

            await @event!.PublishAsync();
        }
    }
}
