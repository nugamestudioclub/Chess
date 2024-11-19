using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        return GetDiagonalMoves(board, Position, teamColor);
    }

    public static List<ChessMove> GetDiagonalMoves(Board board, Vector2Int position, TeamColor teamColor)
    {
        Vector2Int diagonal1 = Vector2Int.right + Vector2Int.up;
        Vector2Int diagonal2 = Vector2Int.right + Vector2Int.down;

        List<ChessMove> moves = new List<ChessMove>();

        AppendLine(position, diagonal1, 1, moves, board, teamColor);
        AppendLine(position, diagonal1 * -1, 1, moves, board, teamColor);
        AppendLine(position, diagonal2, 1, moves, board, teamColor);
        AppendLine(position, diagonal2 * -1, 1, moves, board, teamColor);

        return moves;
    }
}
