using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.VisualDebugging.Unity;
using FeatureProviders;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private Systems _systems;
    private Contexts _contexts;
    
    void Start()
    {
        DontDestroyOnLoad(this);
        _contexts = Contexts.sharedInstance;
        CreateBursh();
        CreateHistory();
        GetSystems(_contexts);
        _systems.Initialize();
    }

    private void CreateHistory()
    {
        _contexts.game.ReplaceHistoryStrokes(new Stack<HistoryBrash[]>());
        _contexts.game.ReplaceStrokeSteps(new StrokeSteps{count = 0});
    }

    private void CreateBursh()
    {
        _contexts.game.ReplaceBrush(Color.blue, 0.1f,0.25f,BrushType.draw);
        _contexts.game.ReplaceBrushPosition(Vector3.zero, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }
    
    private void OnDestroy()
    {
         _systems.TearDown();
    }
    
    private void GetSystems(Contexts contexts)
    {
        _systems = new Systems();
        _systems.Add(new DestroyFeatureProvider().Feature(contexts));
        _systems.Add(new InputFeatureProvider().Feature(contexts));
        _systems.Add((new GameFeatureProvider()).Feature(contexts));
    }
}
