using UnityEngine;

public class Convert : ARedditComment
{
    public override string GetName()
    {
        return "Convert";
    }

    public override string GetDescription()
    {
        return "A bishop on your side converts to a knight";
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        foreach (Piece p in Board.instance.Pieces)
        {
            if (piece.teamColor == p.teamColor && p.pieceType == PieceType.Bishop)
            {
                p.status = RandomStatus.Convert;
                p.pieceType = PieceType.Knight;
                p.UpdateVisual();
                return;
            }
        }

    }
}
