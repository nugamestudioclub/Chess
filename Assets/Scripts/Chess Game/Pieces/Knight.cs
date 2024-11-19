using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        List<ChessMove> moves = new List<ChessMove>();

        AddIfNoTeamate(moves, Position + Vector2Int.up + Vector2Int.up + Vector2Int.right, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.up + Vector2Int.up + Vector2Int.left, board, teamColor);

        AddIfNoTeamate(moves, Position + Vector2Int.left + Vector2Int.left + Vector2Int.up, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.left + Vector2Int.left + Vector2Int.down, board, teamColor);

        AddIfNoTeamate(moves, Position + Vector2Int.down + Vector2Int.down + Vector2Int.right, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.down + Vector2Int.down + Vector2Int.left, board, teamColor);

        AddIfNoTeamate(moves, Position + Vector2Int.right + Vector2Int.right + Vector2Int.up, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.right + Vector2Int.right + Vector2Int.down, board, teamColor);

        return moves;
    }
}
