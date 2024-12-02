using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Vector2Int Position { get; private set; }

    public TeamColor teamColor { get; set; }

    public bool hasMoved = false;
    public bool selected = false;
    public bool hovered = false;

    public void SetPosition(Vector2Int position, Vector3 worldPos)
    {
        Position = position;
        transform.position = worldPos;
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

    private void Update()
    {
        if (this.hovered)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f);
        }
        else
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        }

        if (this.selected)
        {
            var hoveredPieceMat = this.GetComponent<Renderer>().material;
            hoveredPieceMat.SetFloat("_Alpha", 1.0f);
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 0, 90));
        }
        else
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 255, 255));
        }
    }

    public static void AppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board, TeamColor teamColor)
    {
        DoAppendLine(pos, diagonal, step, moves, board, new List<Vector2Int>(), pos, teamColor);
    }

    public static void DoAppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board, List<Vector2Int> path, Vector2Int origin, TeamColor teamColor)
    {
        Vector2Int checkPos = pos + (diagonal * step);

        if (!board.ContainsPosition(checkPos))
        {
            return;
        }

        Piece piece = board.GetPiece(checkPos);

        if (piece != null)
        {
            if (piece.teamColor == teamColor)
            {
                return;
            }
        }

        ChessMove move = new ChessMove()
        {
            origin = new Vector2Int(origin.x, origin.y),
            destination = new Vector2Int(checkPos.x, checkPos.y),
            pathSteps = new List<Vector2Int>(path)
        };

        moves.Add(move);

        path.Add(checkPos);

        if (piece == null)
        {
            DoAppendLine(checkPos, diagonal, step, moves, board, path, origin, teamColor);
        }
    }

    protected void AddIfNoTeamate(List<ChessMove> moves, Vector2Int position, Board board, TeamColor teamColor)
    {
        if (!board.ContainsPosition(position))
        {
            return;
        }

        Piece piece = board.GetPiece(position);

        if (piece != null)
        {
            if (piece.teamColor == teamColor)
            {
                return;
            }
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
