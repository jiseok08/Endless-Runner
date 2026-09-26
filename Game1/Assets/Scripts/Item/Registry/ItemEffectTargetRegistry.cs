using System;
using System.Collections.Generic;

public class ItemEffectTargetRegistry
{
    private readonly Dictionary<Type, object> targets = new Dictionary<Type, object>();

    public void Register<T>(T target) where T : class
    {
        targets[typeof(T)] = target;
    }

    public T Get<T>() where T : class
    { 
        if (targets.TryGetValue(typeof(T), out object target))
        {
            return (T)target;
        }
        
        return null;
    }
}
