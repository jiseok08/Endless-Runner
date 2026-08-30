using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectTargetRegistry
{
    private readonly Dictionary<Type, object> targets = new Dictionary<Type, object>();

    public void Register<T>(T target) where T : class
    {
        Type type = typeof(T);

        if (targets.ContainsKey(type))
        {
            return;
        }
        else
        {
            targets.Add(type, target);
        }
    }

    public T Get<T>() where T : class
    { 
        if (targets.TryGetValue(typeof(T), out object target))
        {
            return target as T;
        }
        
        return null;
    }
}
