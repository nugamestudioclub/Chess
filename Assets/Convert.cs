using UnityEngine;

public class Convert : ARedditComment
{
    public override string GetName()
    {
        return "Convert";
    }

    public override string GetDescription()
    {
        return "A bishop on your side converts to a knight, active until another bishop is encountered";
    }

    public override int GetDuration()
    {
        return -1;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        int count = 0;
        foreach (Piece p in Board.instance.Pieces)
        {
            
            if (p != null && p.pieceType == PieceType.Bishop)
            {
                count++;
            }
        }

        if (count > 0)
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
}
