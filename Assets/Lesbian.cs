using Unity.VisualScripting;
using UnityEngine;

public class Lesbian : ARedditComment
{

    public int duration = 2;
    public override string GetName()
    {
        return "Lesbian";
    }

    public override string GetDescription()
    {
        return "Last played piece turns into a queen";
    }

    public override int GetDuration()
    {
        duration--;
        return duration;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        piece.status = RandomStatus.Lesbian;
        piece.pieceType = PieceType.Queen;
        piece.UpdateVisual();
    }
}
