using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        Vector2Int diagonal1 = Vector2Int.right + Vector2Int.up;
        Vector2Int diagonal2 = Vector2Int.right + Vector2Int.down;

        return new List<ChessMove>();
    }

    private void appendDiagonal(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board, List<Vector2Int> path)
    {
        Vector2Int checkPos = pos + (diagonal * step);

        if (!board.ContainsPosition(checkPos))
        {
            return;
        }

        ChessMove move = new ChessMove()
        {
            origin = Position,
            destination = checkPos,
            pathSteps = path
        };

    }
}
