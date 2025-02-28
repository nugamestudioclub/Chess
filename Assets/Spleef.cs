using UnityEngine;

public class Spleef : ARedditComment
{
    public override string GetName()
    {
        return "Spleef";
    }

    public override string GetDescription()
    {
        return "The tile you just moved from falls into the void";
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        foreach (var sqr in ChessPlayer.instance.squares)
        {
            if (sqr.position == piece.Position)
            {
                sqr.gameObject.SetActive(false);
            }
        }
    }
}
