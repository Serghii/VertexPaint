using Entitas;

namespace FeatureProviders
{
    public class DestroyFeatureProvider : IFeatureProvider
    {
        public Systems Feature(Contexts contexts)
        {
            return new Feature("Destroy feature")
                    .Add(new DestroyEntitiesSystem(contexts))
                ;

        }
    }
}