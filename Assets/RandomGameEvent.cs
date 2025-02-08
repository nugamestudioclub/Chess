using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomGameEvent
{

    // Dictionary to hold descriptions for each status
    private static Dictionary<RandomStatus, string> randomStatusDescriptions = new Dictionary<RandomStatus, string>();

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
    }
    
    // Call a random event for a piece
    public static void CallRandomEvent(Piece piece)
    {
        // Initialize descriptions if not already done
        if (randomStatusDescriptions.Count == 0)
        {
            InitializeDescriptions();
        }
        // Trigger a random event
        if (Random.Range(0, 5) < 1)
        {
            piece.status = RandomStatus.None;
        }
        else
        {
            if (piece.pieceType == PieceType.Bishop)
            {
                piece.status = RandomStatus.Convert;
                piece.pieceType = PieceType.Knight;
            }
            else
            {
                piece.status = RandomStatus.Lesbian;
                piece.pieceType = PieceType.Queen;
            }

        }
         
        Debug.Log("TEAM COLOR BEFORE " + piece.teamColor);
        piece.UpdateVisual();
        Debug.Log("TEAM COLOR AFTER " + piece.teamColor);

        // Print the description of the status
        Debug.Log(randomStatusDescriptions[piece.status]);
    }


}

// Enum representing random statuses
public enum RandomStatus
{
    None,
    Lesbian,
    Convert
}