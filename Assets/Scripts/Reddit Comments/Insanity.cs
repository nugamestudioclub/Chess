using UnityEngine;

public class Insanity : ARedditComment
{
    public override string GetName()
    {
        return "Insanity";
    }

    public override string GetDescription()
    {
        return "50% chance to kill an adjacent piece, ally or not (3 turns) [PRIORITY]";
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
            
            if (Random.Range(0, 2) == 0)
            {
                int randomDirection = Random.Range(0, 4);
                Vector2Int adjacentPiecePosition = Vector2Int.up;
                /*
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

                }
                */
                if (randomDirection == 0)
                {
                    if (Board.instance.HasPiece(piece.Position + Vector2Int.left))
                        adjacentPiecePosition = Vector2Int.left;
                    else
                        randomDirection = 1;
                }
                if (randomDirection == 1)
                {
                    if (Board.instance.HasPiece(piece.Position + Vector2Int.right))
                        adjacentPiecePosition = Vector2Int.left;
                    else
                        randomDirection = 2;
                }
                if (randomDirection == 2)
                {
                    if (Board.instance.HasPiece(piece.Position + Vector2Int.up))
                        adjacentPiecePosition = Vector2Int.left;
                    else
                        randomDirection = 3;
                }
                if (randomDirection == 3)
                {
                    if (Board.instance.HasPiece(piece.Position + Vector2Int.down))
                        adjacentPiecePosition = Vector2Int.left;
                }
                piece.killParticles.Play();
                Board.instance.RemovePiece(piece.Position + adjacentPiecePosition);
            }
            /*
            Board.instance.RemovePiece(piece.Position + Vector2Int.left);
            Board.instance.RemovePiece(piece.Position + Vector2Int.right);
            Board.instance.RemovePiece(piece.Position + Vector2Int.up);
            Board.instance.RemovePiece(piece.Position + Vector2Int.down);
            */
        }
    }
}
