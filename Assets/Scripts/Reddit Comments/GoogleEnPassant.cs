using UnityEngine;

public class GoogleEnPassant : ARedditComment
{
    public override string GetDescription()
    {
        return "Googles 'en passant.'";
    }

    public override int GetDuration()
    {
        return 1;
    }

    public override string GetName()
    {
        return "Google en passant";
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        Application.OpenURL("https://www.google.com/search?q=en+passant");
    }
}
