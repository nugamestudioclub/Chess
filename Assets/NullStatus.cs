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

    public override void SaySomeDumbShit(Piece piece)
    {
        // nothing
    }
}
