using UnityEngine;

public class Blessed : ARedditComment
{
    public override string GetName()
    {
        return "Blessed";
    }

    public override string GetDescription()
    {
        return "Piece becomes a bishop after moving";
    }

    public override int GetDuration()
    {
        return 1;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        piece.blessedParticles.Play();
        piece.pieceType = PieceType.Bishop;
        piece.UpdateVisual();
    }
}
