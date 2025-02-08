using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "Events/EventManager")]
public class EventManager : ScriptableObject
{
    public StatusEffectData statusData;
    [SerializeField] private List<RandomEventSO> events;

    [NonSerialized] private HashSet<RandomEventSO> seenEvents = new();
    [NonSerialized] private HashSet<RandomEvent> currentEvents = new();

    [HideInInspector] public Board board;
    [NonSerialized] private int numEvents = 0;

    [NonSerialized] private int whiteNextEvent = 0;
    public int WhiteNextEvent => whiteNextEvent;
    [NonSerialized] private int blackNextEvent = 0;
    public int BlackNextEvent => blackNextEvent;

    public void SetNextWhiteEvent() => whiteNextEvent = GenNextEventMove();
    public void SetNextBlackEvent() => blackNextEvent = GenNextEventMove();

    private int GenNextEventMove()
    {
        var rand = new System.Random();
        int moves = rand.Next(6);
        return board.NumMoves + moves + 1;
    }

    public void RemoveEvent(RandomEvent evt)
    {
        currentEvents.Remove(evt);
    }

    public void AddEvent<Event>(Event evt, RandomEventSO spawner) where Event : RandomEvent
    {
        currentEvents.Add(evt);
        seenEvents.Add(spawner);

        numEvents++;

        evt.Start();
    }

    private List<int> availableEvents = new();


    public void CalcAvailable()
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (availableEvents.Contains(i))
            {
                continue;
            }

            var evt = events[i];
            if (evt != null)
            {
                Debug.Log(evt.name);

                if (evt.ExcludeBeforeMove > board.NumMoves)
                {
                    continue;
                }

                if (evt.ExcludeBeforeEventCount > numEvents)
                {
                    continue;
                }

                if (!ValidatePrereqs(evt))
                {
                    continue;
                }

                availableEvents.Add(i);
            }
        }
    }

    public List<int> ScreenAvailable()
    {
        var finalAvail = new List<int>();

        foreach (var availIndex in availableEvents)
        {
            if (!ValidateCoreqs(events[availIndex]))
            {
                continue;
            }

            finalAvail.Add(availIndex);
        }

        return finalAvail;
    }

    private bool ValidatePrereqs(RandomEventSO evt)
    {
        foreach (var prereq in evt.prerequEvents)
        {
            if (!seenEvents.Contains(prereq))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidateCoreqs(RandomEventSO evt)
    {
        foreach (var coreq in evt.coreqEvents)
        {
            if (!ValidateCoreq(coreq))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidateCoreq(RandomEventSO coreq)
    {
        foreach (var curEvt in currentEvents)
        {
            if (curEvt.spawnedBy.Equals(coreq))
            {
                return true;
            }
        }

        return false;
    }
}