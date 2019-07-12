//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public HistoryBrashComponent historyBrash { get { return (HistoryBrashComponent)GetComponent(GameComponentsLookup.HistoryBrash); } }
    public bool hasHistoryBrash { get { return HasComponent(GameComponentsLookup.HistoryBrash); } }

    public void AddHistoryBrash(HistoryBrash newValue) {
        var index = GameComponentsLookup.HistoryBrash;
        var component = (HistoryBrashComponent)CreateComponent(index, typeof(HistoryBrashComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceHistoryBrash(HistoryBrash newValue) {
        var index = GameComponentsLookup.HistoryBrash;
        var component = (HistoryBrashComponent)CreateComponent(index, typeof(HistoryBrashComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveHistoryBrash() {
        RemoveComponent(GameComponentsLookup.HistoryBrash);
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

    static Entitas.IMatcher<GameEntity> _matcherHistoryBrash;

    public static Entitas.IMatcher<GameEntity> HistoryBrash {
        get {
            if (_matcherHistoryBrash == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.HistoryBrash);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHistoryBrash = matcher;
            }

            return _matcherHistoryBrash;
        }
    }
}
