using System.Collections.Generic;
using Entitas;

public interface IReactiveHandledEntity : IEntity, IReactiveHandlerEntity
{
}

public partial class GameEntity : IReactiveHandledEntity
{
}

public partial class InputEntity : IReactiveHandledEntity
{
}

public class ExecuteReactiveHandlersSystem : IExecuteSystem
{
    private IGroup[] _groups;

    public ExecuteReactiveHandlersSystem(Contexts contexts)
    {
        _groups = new IGroup[]
        {
            contexts.game.GetGroup(GameMatcher.ReactiveHandler),
            contexts.input.GetGroup(InputMatcher.ReactiveHandler),
        };
    }

    public void Execute()
    {
        if (_groups.Length == 0)
            return;

        foreach (var g in _groups)
        {
            switch (g)
            {
                case Group<GameEntity> gameGroup:
                {
                    foreach (var e in gameGroup.AsEnumerable() as IEnumerable<IReactiveHandledEntity>)
                    {
                        if (e.hasReactiveHandler)
                            e.reactiveHandler.handler.Execute();
                    }

                    continue;
                }

                case Group<InputEntity> inputGroup:
                {
                    foreach (var e in inputGroup.AsEnumerable())
                    {
                        if (e.hasReactiveHandler)
                            e.reactiveHandler.handler.Execute();
                    }

                    continue;
                }
            }
        }
    }
    
}