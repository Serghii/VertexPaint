using Entitas;


public class GameFeatureProvider //:  IFeatureProvider
{
    public Systems Feature(Contexts contexts)
    {
        return new Feature("Game feature")
            .Add(new DrawSystem(contexts))
            .Add(new PeekSystem(contexts))
            .Add(new UndoSystem(contexts));
    }
}