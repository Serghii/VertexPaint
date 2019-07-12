using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;


[Game, Unique]
public class GameIdIndex : IdIndex<GameEntity>
{
    public GameIdIndex(Context<GameEntity> context, IMatcher<GameEntity> matcher) : base(context, matcher)
    {
		
    }
}

public class IdIndex<TEntity> where TEntity : Entity {
	
    IGroup<TEntity> observedCollection;
    Dictionary<string, TEntity> lookup = new Dictionary<string, TEntity>();

    public IdIndex(Context<TEntity> context, IMatcher<TEntity> matcher){
        observedCollection = context.GetGroup(matcher);

        observedCollection.OnEntityAdded += AddEntity;
        observedCollection.OnEntityRemoved += RemoveEntity;
    }

    ~IdIndex()
    {
        Cleanup();
    }
	
    public void Cleanup()
    {
        observedCollection.OnEntityAdded -= AddEntity;
        observedCollection.OnEntityRemoved -= RemoveEntity;
        lookup.Clear();
    }


    public TEntity FindEntityWithId(string id)
    {
        if (!lookup.ContainsKey(id))
            return null;
        return lookup[id];
    }

    protected virtual void AddEntity(IGroup collection, TEntity entity, int index, IComponent component)
    {
        var idComponent = component as IdComponent;
        if(idComponent != null && !string.IsNullOrEmpty(idComponent.value)){
            if(lookup.ContainsKey(idComponent.value) && lookup[idComponent.value] == entity){
                return;
            }

            if(lookup.ContainsKey(idComponent.value) && lookup[idComponent.value] != entity){
                throw new Exception(
                    "the key " + idComponent.value + 
                    " is not unique. Present on entity: " + entity.creationIndex + 
                    " and entity: " + lookup[idComponent.value].creationIndex);
            }
            entity.Retain(this);
            lookup[idComponent.value] = entity;
        }

    }

    protected virtual void RemoveEntity(IGroup collection, TEntity entity, int index, IComponent component)
    {
        var idComponent = component as IdComponent;
        if(idComponent != null && lookup.ContainsKey(idComponent.value)){
            lookup[idComponent.value].Release(this);
            lookup.Remove(idComponent.value);
        }
    }
}

public static class Math
{
    public static float LerpFromTo(float start, float end, float value)
    {
        return start + value * (end-start);
    }

    public static float DeLerpFromTo(float start, float end, float value)
    {
        return 1f / (end - start) * (value - start);
    }
}