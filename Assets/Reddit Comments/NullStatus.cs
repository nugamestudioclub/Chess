using UnityEngine;

public class NullStatus : ARedditComment
{
    public override string GetName()
    {
        return "Null";
    }

    public override string GetDescription()
    {
        return "Does nothing";
    }

    public override int GetDuration()
    {
        return -1;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        piece.nullParticles.Play();
    }
}
