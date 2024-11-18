using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Vector2Int Position { get; private set; }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
    }

    public virtual List<ChessMove> GetPossibleMoves(Board board)
    {
        return GetDefaultMoves(board);
    }

    public abstract List<ChessMove> GetDefaultMoves(Board board);

    public virtual void GameUpdate()
    {
        return;
    }
}
