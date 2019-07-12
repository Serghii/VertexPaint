using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Input]
public class IdComponent : IComponent
{
    [PrimaryEntityIndex]
    public string value;
}