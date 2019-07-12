using System.Collections.Generic;
using Entitas;

public interface IDestroyableEntity : IEntity, IDestroyEntity, IDestroyImmediateEntity
{
}

public partial class GameEntity : IDestroyableEntity
{
}

public partial class InputEntity : IDestroyableEntity
{
}

public class DestroyEntitiesSystem : MultiReactiveSystem<IDestroyableEntity, Contexts>
{
    public DestroyEntitiesSystem(Contexts contexts) : base(contexts)
    {
    }

    protected override ICollector[] GetTrigger(Contexts contexts)
    {
        return new ICollector[]
        {
            contexts.game.CreateCollector(GameMatcher.DestroyImmediate),
            contexts.game.CreateCollector(GameMatcher.Destroy),

            contexts.input.CreateCollector(InputMatcher.DestroyImmediate),
            contexts.input.CreateCollector(InputMatcher.Destroy),
        };
    }

    protected override bool Filter(IDestroyableEntity entity)
    {
        return entity.isEnabled && (entity.isDestroy || entity.isDestroyImmediate);
    }

    protected override void Execute(List<IDestroyableEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.isDestroyImmediate)
                DestroyImmediate(entity);
            else if (entity.isDestroy)
                Destroy(entity);
        }
    }

    private void Destroy(IDestroyableEntity entity)
    {
        entity.isDestroyImmediate = true;
    }

    private void DestroyImmediate(IDestroyableEntity entity)
    {
        if (entity.isEnabled)
            entity.Destroy();
    }
}