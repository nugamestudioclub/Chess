using UnityEngine;

public class RemoveDEI : ARedditComment
{
    public override string GetName()
    {
        return "RemoveDEI";
    }

    public override string GetDescription()
    {
        return "All queens become kings";
    }

    public override int GetDuration()
    {
        return 1;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        foreach (Piece p in Board.instance.Pieces)
        {
            if (p == null)
            {
                continue;
            }

            if (p.pieceType == PieceType.Queen)
            {
                piece.pieceType = PieceType.King;
                piece.UpdateVisual();
            }
        }
    }
}
