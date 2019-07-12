using Entitas;
using UnityEngine;

public class CommonFeatureProvider :  IFeatureProvider
{
    public Systems Feature(Contexts contexts)
    {
        return new Feature("Common")
            .Add(new ExecuteReactiveHandlersSystem(contexts)
            );
    }
}