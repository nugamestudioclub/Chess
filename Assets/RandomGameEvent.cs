using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomGameEvent
{

    public static List<ARedditComment> redditComments { get; private set; } = new List<ARedditComment>();

    // Dictionary to hold descriptions for each status
    public static Dictionary<RandomStatus, string> randomStatusDescriptions  { get; private set; } = new Dictionary<RandomStatus, string>();

    // Custom method to get the description for a specific status
    public static string GetDescription(RandomStatus status)
    {
        // Initialize descriptions if not already done
        if (randomStatusDescriptions.Count == 0)
        {
            InitializeDescriptions();
        }
        
        // Check if the status exists in the dictionary
        if (randomStatusDescriptions.ContainsKey(status))
        {
            return randomStatusDescriptions[status];
        }
        else
        {
            return "Description not found for this status.";  // Default message if status is not found
        }
    }
    
    // Method to initialize the status descriptions
    public static void InitializeDescriptions()
    {
        randomStatusDescriptions[RandomStatus.None] = "No status effects";
        randomStatusDescriptions[RandomStatus.Lesbian] = "Piece turns into a Queen";
        randomStatusDescriptions[RandomStatus.Convert] = "Bishop turns into a Knight";
        randomStatusDescriptions[RandomStatus.Spleef] = "The next time this piece moves, it's square will fall away";
    }
    
    public static void InitializeRedditComments()
    {
        redditComments.Clear();
        redditComments.Add(new Convert());
        redditComments.Add(new Lesbian());
        redditComments.Add(new  Spleef());
        redditComments.Add(new  NullStatus());
    }


    public static void CallRandomEvent(Piece piece, int randomComment)
    {
        // Initialize descriptions if not already done
        if (randomStatusDescriptions.Count == 0)
        {
            InitializeDescriptions();
        }


        if (redditComments.Count == 0)
        {
            InitializeRedditComments();
        }


        redditComments[randomComment].SaySomeDumbShit(piece);
        // change UI (NOT SUPPOSED TO BE HERE)
        ActiveEventUI.instance.SetActiveEventText(redditComments[randomComment].GetName(), redditComments[randomComment].GetDescription());
        // square selector position


        Debug.Log("TEAM COLOR BEFORE " + piece.teamColor);
        piece.UpdateVisual();
        Debug.Log("TEAM COLOR AFTER " + piece.teamColor);

        // Print the description of the status
        Debug.Log(randomStatusDescriptions[piece.status]);
    }



    public static int nextRandomEventIndex { get; private set; } = 0;


    // Call a random event for a piece
    public static int GetNextRandomEvent()
    {
        nextRandomEventIndex = Random.Range(0, redditComments.Count);
        return nextRandomEventIndex;
    }




}

// Enum representing random statuses
public enum RandomStatus
{
    None,
    Lesbian,
    Spleef,
    Convert
}