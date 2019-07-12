using System.Diagnostics;
using System.Net;
using Entitas;
using UnityEngine;

[Game]
public struct DrawableComponent : IComponent
{
    
    public Transform transform;
    public int InstanceId;
    public Mesh mesh;
    public Color[] originalColors;
    public Color[] debugColors;
    public Vector3[] vertices;
    
}