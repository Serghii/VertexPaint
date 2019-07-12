using Entitas;


public class InputFeatureProvider :  IFeatureProvider
{
    public Systems Feature(Contexts contexts)
    {
        return new Feature("Input feature")
            .Add(new UserInputSystem(contexts)
            );
    }
}