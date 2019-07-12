using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;


public class DrawSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    private IGroup<GameEntity> BrashHistory;

    public DrawSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        BrashHistory = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.HistoryBrash).NoneOf(GameMatcher.Destroy));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.MouseHit);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDrawable && entity.drawable.transform != null && entity.hasMouseHit &&
               Input.GetMouseButton(0) && _contexts.game.brush.brushType == BrushType.draw;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            Transform transform = entity.drawable.transform;
            Vector3[] vertices = entity.drawable.vertices;
            Brush brush = _contexts.game.brush;
            Color[] debugColors = entity.drawable.debugColors;
            Vector3 hitPos = entity.mouseHit.value;
            HistoryCheck(entity.drawable);
            for (int i = 0; i < vertices.Length; i++)
            {
                if (debugColors[i] == brush.gui_BrushColor)
                    continue;
                Vector3 vertPos = Vector3.Scale(vertices[i], transform.localScale);
                float mag = (vertPos - hitPos).magnitude;
                if (mag > brush.brushSize)
                    continue;

                debugColors[i] = Color.Lerp(debugColors[i], brush.gui_BrushColor, brush.gui_BrushOpacity);
            }

            entity.drawable.mesh.colors = debugColors;
        }
    }

    private void HistoryCheck(DrawableComponent drawable)
    {
        int instanceId = drawable.InstanceId;
        var e = BrashHistory.GetEntities().FirstOrDefault(i => i.historyBrash.value.instanceId == instanceId);
        if (e == null)
        {
            e = _contexts.game.CreateEntity();
            e.AddHistoryBrash(new HistoryBrash(_contexts.game.strokeSteps.value.count, drawable.InstanceId,
                drawable.mesh.colors));
        }
    }
}

public class PeekSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;

    public PeekSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.MouseHit);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDrawable && entity.drawable.transform != null && entity.hasMouseHit &&
               Input.GetMouseButton(0) && _contexts.game.brush.brushType == BrushType.peek;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            Transform transform = entity.drawable.transform;
            Vector3[] vertices = entity.drawable.vertices;
            Vector3 hitPos = entity.mouseHit.value;
            // find index of closest vertex to mousehit
            int index = -1;
            var minDistance = float.MaxValue;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertPos = Vector3.Scale(vertices[i], transform.localScale);
                float distance = (vertPos - hitPos).magnitude;
                if (minDistance > distance)
                {
                    index = i;
                    minDistance = distance;
                }
            }
            //no find
            if (index == -1)
            {
                Debug.Log($"cannot find");
                return;
            }
            // set to brushColor vertex color
            Color[] debugColors = entity.drawable.debugColors;
            var brush = _contexts.game.brush;
            brush = new Brush
            {
                brushSize = brush.brushSize,
                gui_BrushColor = debugColors[index],
                gui_BrushOpacity = brush.gui_BrushOpacity,
                brushType = brush.brushType
            };
            _contexts.game.ReplaceBrush(brush.gui_BrushColor, brush.brushSize, brush.gui_BrushOpacity, brush.brushType);


        }
    }
}