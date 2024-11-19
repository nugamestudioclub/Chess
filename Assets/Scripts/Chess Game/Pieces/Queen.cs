using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        var bishopMoves = Bishop.GetDiagonalMoves(board, Position, teamColor);

        bishopMoves.AddRange(Rook.GetRookMoves(board, Position, teamColor));

        return bishopMoves;
    }
}
