using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public class UndoSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    private IGroup<GameEntity> _drawable;

    public UndoSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _drawable = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Drawable).NoneOf(GameMatcher.Destroy));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Undo);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isUndo;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (_contexts.game.historyStrokes.value.Count == 0)
            {
                Debug.Log($"no any Undo");
                UpdateStrokeSteps();
                return;
            }

            HistoryBrash[] strokes = _contexts.game.historyStrokes.value.Pop();
           
            foreach (var stroke in strokes)
            { 
                // find undo object
                GameEntity drawableObject = _drawable.GetEntities()
                    .FirstOrDefault(i => i.drawable.InstanceId == stroke.instanceId);
                if (drawableObject == null)
                {
                    Debug.Log($"cannot find undoObject in Scene objects");
                    return;
                }

                // Aply undoColors on object
                drawableObject.drawable.mesh.colors = stroke.colors;
                UpdateDrawableComponent(drawableObject);
            }
        }
        UpdateStrokeSteps();
    }

    private void UpdateStrokeSteps()
    {
        int step = _contexts.game.strokeSteps.value.count;
        step = --step < 0 ? 0 : step--;
        _contexts.game.strokeStepsEntity.ReplaceStrokeSteps(new StrokeSteps {count = step});
    }

    private void UpdateDrawableComponent(GameEntity drawable)
    {
        var transform = drawable.drawable.transform;
        var mesh = drawable.drawable.mesh;
        var vert = drawable.drawable.vertices;
        var origColors = drawable.drawable.originalColors;
        drawable.ReplaceDrawable(transform, transform.GetInstanceID(), mesh, origColors, mesh.colors, vert);
    }
}