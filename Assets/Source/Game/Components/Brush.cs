using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game,Unique]
public struct Brush : IComponent
{
    public Color gui_BrushColor;
    public float brushSize;
    public float gui_BrushOpacity;
    public BrushType brushType;
}

[Game,Unique]
public struct BrushPosition : IComponent
{
    public Vector3 position;
    public Vector3 normal;
}

public enum BrushType
{
    draw,
    peek
}