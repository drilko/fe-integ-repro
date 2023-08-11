namespace FE.IntegrationTests.Repro.API.Core;

public abstract record BaseDto<TDto, TEntity> : IRegister
where TDto : class, new()
where TEntity : class
{
    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TDto).Adapt(entity);
    }

    public static TDto FromEntity(TEntity entity)
    {
        return entity.Adapt<TDto>();
    }

    private TypeAdapterConfig? Config { get; set; }

    public virtual void AddCustomMappings() { }

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMappings();
    }
    protected TypeAdapterSetter<TDto, TEntity> SetCustomMappings()
        => Config!.ForType<TDto, TEntity>();

    protected TypeAdapterSetter<TEntity, TDto> SetCustomMappingsInverse()
        => Config!.ForType<TEntity, TDto>();
}
