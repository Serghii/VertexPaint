using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game,Unique]
public struct HistoryStrokes : IComponent
{
    public Stack<HistoryBrash[]> value;
}

[Game]
public struct HistoryBrash
{
    public int step;
    public int instanceId;
    public Color[] colors;

    public HistoryBrash(int step,int instanceId,Color[]colors)
    {
        this.step = step;
        this.instanceId = instanceId;
        this.colors = colors;
    }
}

[Game,Unique]
public struct StrokeSteps
{
    public int count;
}
