using UnityEngine;

public class Spleef : ARedditComment
{
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
