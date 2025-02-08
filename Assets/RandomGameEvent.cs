using UnityEngine;

public class RandomGameEvent : MonoBehaviour
{
    
    public void CallRandomEvent(Piece piece)
    {
        if (Random.Range(0, 2) > 1)
        {
            Debug.Log("Default event called");
            piece.status = RandomStatus.None;
        }
        else
        {
            piece.pieceType = PieceType.Queen;
            piece.UpdateVisual();
            piece.status = RandomStatus.Lesbian;
        }
    }
    
}

public enum RandomStatus
{
    None,
    Lesbian
}