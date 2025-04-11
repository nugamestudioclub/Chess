using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "Events/EventManager")]
public class EventManager : ScriptableObject
{
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
    
    private List<int> availableEvents = new();

    
}