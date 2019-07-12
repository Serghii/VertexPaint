using Entitas;

[Game, Input]
public sealed class ReactiveHandlerComponent : IComponent
{
    public IExecuteSystem handler;
}