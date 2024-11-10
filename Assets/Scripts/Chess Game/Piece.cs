using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Vector2Int Position { get; private set; }

    public void setPosition(Vector2Int position)
    {
        Position = position;
    }

    public abstract List<ChessMove> GetPossibleMoves(Board board);

    public virtual void GameUpdate()
    {
        return;
    }
}
