//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity gameIdIndexEntity { get { return GetGroup(GameMatcher.GameIdIndex).GetSingleEntity(); } }
    public GameIdIndexComponent gameIdIndex { get { return gameIdIndexEntity.gameIdIndex; } }
    public bool hasGameIdIndex { get { return gameIdIndexEntity != null; } }

    public GameEntity SetGameIdIndex(GameIdIndex newValue) {
        if (hasGameIdIndex) {
            throw new Entitas.EntitasException("Could not set GameIdIndex!\n" + this + " already has an entity with GameIdIndexComponent!",
                "You should check if the context already has a gameIdIndexEntity before setting it or use context.ReplaceGameIdIndex().");
        }
        var entity = CreateEntity();
        entity.AddGameIdIndex(newValue);
        return entity;
    }

    public void ReplaceGameIdIndex(GameIdIndex newValue) {
        var entity = gameIdIndexEntity;
        if (entity == null) {
            entity = SetGameIdIndex(newValue);
        } else {
            entity.ReplaceGameIdIndex(newValue);
        }
    }

    public void RemoveGameIdIndex() {
        gameIdIndexEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public GameIdIndexComponent gameIdIndex { get { return (GameIdIndexComponent)GetComponent(GameComponentsLookup.GameIdIndex); } }
    public bool hasGameIdIndex { get { return HasComponent(GameComponentsLookup.GameIdIndex); } }

    public void AddGameIdIndex(GameIdIndex newValue) {
        var index = GameComponentsLookup.GameIdIndex;
        var component = (GameIdIndexComponent)CreateComponent(index, typeof(GameIdIndexComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGameIdIndex(GameIdIndex newValue) {
        var index = GameComponentsLookup.GameIdIndex;
        var component = (GameIdIndexComponent)CreateComponent(index, typeof(GameIdIndexComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGameIdIndex() {
        RemoveComponent(GameComponentsLookup.GameIdIndex);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherGameIdIndex;

    public static Entitas.IMatcher<GameEntity> GameIdIndex {
        get {
            if (_matcherGameIdIndex == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GameIdIndex);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGameIdIndex = matcher;
            }

            return _matcherGameIdIndex;
        }
    }
}
