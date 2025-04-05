using UnityEngine;

public class Insanity : ARedditComment
{
    public override string GetName()
    {
        return "Insanity";
    }

    public override string GetDescription()
    {
        return "33% chance to kill an adjacent piece, ally or not (3 turns) [PRIORITY]";
    }

    public override int GetDuration()
    {
        return 3;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        bool adjacentPieceExists = null != 
                                   Board.instance.GetPiece(piece.Position + Vector2Int.left) ||
                                   Board.instance.GetPiece(piece.Position + Vector2Int.right) ||
                                   Board.instance.GetPiece(piece.Position + Vector2Int.up) ||
                                   Board.instance.GetPiece(piece.Position + Vector2Int.down);
        if (adjacentPieceExists)
        {
            if (Random.Range(0, 3) == 0)
            {
                /*int randomDirection = Random.Range(0, 4);
                Vector2Int adjacentPiecePosition = Vector2Int.up;
                switch (randomDirection)
                {
                    case 0:
                        adjacentPiecePosition = Vector2Int.left;
                        break;
                    case 1:
                        adjacentPiecePosition = Vector2Int.right;
                        break;
                    case 2:
                        adjacentPiecePosition = Vector2Int.up;
                        break;
                    case 3:
                        adjacentPiecePosition = Vector2Int.down;
                        break;

                }*/

            }
            piece.killParticles.Play();
            //Board.instance.RemovePiece(piece.Position + adjacentPiecePosition);
            Board.instance.RemovePiece(piece.Position + Vector2Int.left);
            Board.instance.RemovePiece(piece.Position + Vector2Int.right);
            Board.instance.RemovePiece(piece.Position + Vector2Int.up);
            Board.instance.RemovePiece(piece.Position + Vector2Int.down);

        }
    }
}
