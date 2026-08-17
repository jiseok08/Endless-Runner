using System;
using System.Collections.Generic;

public enum Condition
{ 
    START,
    FINISH,
    RESET
}

public static class State
{
    private static readonly Dictionary<Condition, Action> events = new();

    public static void Subscribe(Condition condition, Action action)
    {
        if(events.ContainsKey(condition)) events[condition] += action;
        else events[condition] = action;
    }

    public static void UnSubscribe(Condition condition, Action action)
    {
        if(events.ContainsKey(condition)) events[condition] -= action;
    }

    public static void Publish(Condition condition)
    {
        if(events.TryGetValue(condition, out var action)) action?.Invoke();
    }
}