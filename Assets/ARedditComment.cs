using UnityEngine;

public abstract class ARedditComment
{
    public abstract string GetName();

    public abstract string GetDescription();

    public abstract int GetDuration();
    
    public abstract void SaySomeDumbShit(Piece piece);
    
}
