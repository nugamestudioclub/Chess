using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ScriptableObject
{
    [SerializeField] private List<RandomEventSO> events;

    private HashSetOfEventTypes seenEvents = new();
    private HashSet<RandomEvent> currentEvents = new();

    public void RemoveEvent(RandomEvent evt)
    {
        currentEvents.Remove(evt);
    }

    public void AddEvent<Event>(Event evt) where Event : RandomEvent
    {
        currentEvents.Add(evt);
        seenEvents.Add<Event>();
    }

    private List<int> availableEvents = new();

    //public RandomEvent
}