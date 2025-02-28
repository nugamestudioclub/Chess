using UnityEngine;

public class Lesbian : ARedditComment
{
    public override string GetName()
    {
        return "Lesbian";
    }

    public override string GetDescription()
    {
        return "Last played piece turns into a queen";
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        piece.status = RandomStatus.Lesbian;
        piece.pieceType = PieceType.Queen;
        piece.UpdateVisual();
    }
}
