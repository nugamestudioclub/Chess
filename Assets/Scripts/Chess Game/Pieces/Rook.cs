using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        return GetRookMoves(board, Position, teamColor);
    }

    public static List<ChessMove> GetRookMoves(Board board, Vector2Int position, TeamColor teamColor)
    {
        Vector2Int right = Vector2Int.right;
        Vector2Int up = Vector2Int.up;

        List<ChessMove> moves = new List<ChessMove>();

        AppendLine(position, right, 1, moves, board, teamColor);
        AppendLine(position, right * -1, 1, moves, board, teamColor);
        AppendLine(position, up, 1, moves, board, teamColor);
        AppendLine(position, up * -1, 1, moves, board, teamColor);

        return moves;
    }
}
