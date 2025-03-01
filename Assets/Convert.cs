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
        if (Board.instance == null)
        {
            Debug.LogError("Board.instance is null!");
            return;
        }

        if (Board.instance.Pieces == null)
        {
            Debug.LogError("Board.instance.Pieces is null!");
            return;
        }

        int count = 0;
        foreach (Piece p in Board.instance.Pieces)
        {
            if (p == null)
            {
                //Debug.LogWarning("Found a null piece in Board.instance.Pieces");
                continue; // Skip null pieces
            }

            if (p.pieceType == PieceType.Bishop)
            {
                count++;
            }
        }

        if (count > 0)
        {
            foreach (Piece p in Board.instance.Pieces)
            {
                if (p == null)
                {
                    //Debug.LogWarning("Found a null piece while converting.");
                    continue;
                }

                if (piece.teamColor == p.teamColor && p.pieceType == PieceType.Bishop)
                {
                    Debug.Log($"Converting {p} to Knight.");

                    p.status = RandomStatus.Convert;
                    p.pieceType = PieceType.Knight;
                    p.UpdateVisual();

                    if (p.convertParticles == null)
                    {
                        Debug.LogError($"convertParticles is null for {p.name}");
                    }
                    else
                    {
                        p.convertParticles.Play();
                    }
                
                    return;
                }
            }
        }
    }


}
