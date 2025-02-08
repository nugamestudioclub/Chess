using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class RandomEventSO : ScriptableObject
{
    public abstract RandomEvent LinkedEventGen(EventManager manager, TeamColor tc, Board board);

    public List<RandomEventSO> prerequEvents;
    public List<RandomEventSO> coreqEvents;

    [SerializeField] private int excludeBeforeMove = 0; // before this move, event cannot be selected

    [SerializeField]
    private int
        excludeBeforeEventCount = 0; // event can only be triggered after this number of events have been triggered

    public int ExcludeBeforeMove => excludeBeforeMove;
    public int ExcludeBeforeEventCount => excludeBeforeEventCount;
}

public abstract class LinkRandomSO<Event> : RandomEventSO where Event : RandomEvent, new()
{
    public override RandomEvent LinkedEventGen(EventManager manager, TeamColor tc, Board board) =>
        RandomEvent.Gen<Event>(manager, this, tc, board);
}

public abstract class RandomEvent
{
    public static RandomEvent Gen<EventType>(EventManager manager, RandomEventSO spawner, TeamColor tc, Board board)
        where EventType : RandomEvent, new()
    {
        EventType evt = new();
        evt.manager = manager;
        evt.spawnedBy = spawner;
        evt.team = tc;
        evt.board = board;
        return evt;
    }

    public EventManager manager;
    public RandomEventSO spawnedBy;
    public TeamColor team;
    public Board board;

    public void Start()
    {
        DoStart();
    }

    public void End()
    {
        DoEnd();

        manager.RemoveEvent(this);
    }

    protected abstract void DoStart();
    protected abstract void DoEnd();
}

public class ListOfEventTypes
{
    private List<Type> typeList = new();

    public void Add<U>() where U : RandomEvent
    {
        typeList.Add(typeof(U));
    }

    public int Count => typeList.Count;

    public Type Get(int index)
    {
        if (index < Count)
        {
            return typeList[index];
        }

        throw new IndexOutOfRangeException();
    }
}

public class HashSetOfEventTypes
{
    private HashSet<Type> typeSet = new();

    public void Add<U>() where U : RandomEvent
    {
        typeSet.Add(typeof(U));
    }

    public int Count => typeSet.Count;

    public bool Has(Type type) => typeSet.Contains(type);
}