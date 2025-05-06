using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity<DerivedType> : MonoBehaviour
    where DerivedType : Entity<DerivedType>
{
    private readonly Dictionary<Type, IEntityComponent<DerivedType>> componentDictionary = new();
    private bool isComponentInitialized;

    //initializing at start because of Default Awake calls
    protected virtual void Start()
    {
        isComponentInitialized = false;
        IEntityComponent<DerivedType>[] componentCollection =
            GetComponentsInChildren<IEntityComponent<DerivedType>>(true);

        foreach (IEntityComponent<DerivedType> item in componentCollection)
        {
            InitializeEntityComponent(item);
        }
        foreach (IEntityComponentStart<DerivedType> item in componentCollection.OfType<IEntityComponentStart<DerivedType>>())
        {
            item.EntityComponentStart(this as DerivedType);
        }
        isComponentInitialized = true;
    }
    private void InitializeEntityComponent(IEntityComponent<DerivedType> component)
    {
        componentDictionary.Add(component.GetType(), component);

        if (component is IEntityComponentAwake<DerivedType> instance)
            instance.EntityComponentAwake(this as DerivedType);
    }
    public EntityComponentType GetEntityComponent<EntityComponentType>()
        where EntityComponentType : MonoBehaviour, IEntityComponent<DerivedType>
    {
        Type targetType = typeof(EntityComponentType);
        Debug.Assert(isComponentInitialized, $"trying to reference {targetType.Name} when it's not initilaized");

        if (componentDictionary.TryGetValue(targetType, out IEntityComponent<DerivedType> value))
            return value as EntityComponentType;

        Debug.LogError($"can't find {targetType.Name}, ReInitializing...", this);

        IEntityComponent<DerivedType> missingInstance = GetComponentInChildren<EntityComponentType>(true);
        if (missingInstance == null)
        {
            Debug.LogError("can't find uninitialized component", this);
            return null;
        }

        InitializeEntityComponent(missingInstance);
        if (missingInstance is IEntityComponentStart<DerivedType> startInstance)
            startInstance.EntityComponentStart(this as DerivedType);

        return missingInstance as EntityComponentType;
    }

}