using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        List<ChessMove> moves = new List<ChessMove>();

        Vector2Int direction = teamColor == TeamColor.White ? new Vector2Int(0, 1) : new Vector2Int(0, -1);

        Vector2Int step = Position + direction;

        if (!board.ContainsPosition(step))
        {
            return moves;
        }

        if (board.GetPiece(step) == null)
        {
            ChessMove move = new ChessMove()
            {
                destination = new Vector2Int(step.x, step.y),
                origin = new Vector2Int(Position.x, Position.y),
                pathSteps = new List<Vector2Int>()
            };

            moves.Add(move);

            if (!hasMoved)
            {
                ChessMove doubleMove = new ChessMove()
                {
                    destination = step + direction,
                    origin = new Vector2Int(Position.x, Position.y),
                    pathSteps = new List<Vector2Int>() { step }
                };

                moves.Add(doubleMove);
            }
        }

        AddIfEnemy(step + Vector2Int.left, board, moves);
        AddIfEnemy(step + Vector2Int.right, board, moves);

        return moves;
    }

    private void AddIfEnemy(Vector2Int position, Board board, List<ChessMove> moves)
    {
        if (!board.ContainsPosition(position))
        {
            return;
        }

        Piece piece = board.GetPiece(position);

        if (piece == null)
        {
            return;
        }

        if (piece.teamColor == teamColor)
        {
            return;
        }

        ChessMove move = new ChessMove()
        {
            destination = new Vector2Int(position.x, position.y),
            origin = new Vector2Int(Position.x, Position.y),
            pathSteps = new List<Vector2Int>()
        };

        moves.Add(move);
    }
}
