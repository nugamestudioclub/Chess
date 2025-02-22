using UnityEngine;

public class Lesbian : ARedditComment
{
    public override void SaySomeDumbShit(Piece piece)
    {
        piece.status = RandomStatus.Lesbian;
        piece.pieceType = PieceType.Queen;
        piece.UpdateVisual();
    }
}
