using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : Piece
{

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private Material whitePieceMaterial;
    [SerializeField] private Material blackPieceMaterial;

    public List<RankToMesh> rankToMeshes = new List<RankToMesh>();
    
    private void OnEnable()
    {
        UpdateVisual();
    }

    private void OnValidate()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter is not assigned on " + gameObject.name);
            return;
        }
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer is not assigned on " + gameObject.name);
            return;
        }

        RankToMesh rankToMesh = rankToMeshes.Find(rtm => rtm.rank == pieceType);
        if (rankToMesh.mesh != null)
        {
            meshFilter.mesh = rankToMesh.mesh;
        }
        else
        {
            Debug.LogError($"No mesh found for {pieceType} in {gameObject.name}");
        }

        switch (teamColor)
        {
            case TeamColor.White:
                meshRenderer.material = whitePieceMaterial;
                break;
            case TeamColor.Black:
                meshRenderer.material = blackPieceMaterial;
                break;
        }
    }

    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        List<ChessMove> pieceMoves = new List<ChessMove>();
        
        switch (pieceType)
        {
            case PieceType.Pawn:
                pieceMoves = GetPawnMoves(board);
                break;
            case PieceType.Rook:
                pieceMoves = GetRookMoves(board);
                break;
            case PieceType.Knight:
                pieceMoves = GetKnightMoves(board);
                break;
            case PieceType.Bishop:
                pieceMoves = GetBishopMoves(board);
                break;
            case PieceType.Queen:
                pieceMoves = GetQueenMoves(board);
                break;
            case PieceType.King:
                pieceMoves = GetKingMoves(board);
                break;
        }

        return pieceMoves;
    }

    private List<ChessMove> GetPawnMoves(Board board)
    {
        List<ChessMove> moves = new List<ChessMove>();
        Vector2Int direction = teamColor == TeamColor.White ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
        Vector2Int step = Position + direction;

        if (!board.ContainsPosition(step)) return moves;

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
                    pathSteps = new List<Vector2Int> { step }
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

    private List<ChessMove> GetRookMoves(Board board)
    {
        return Rook.GetRookMoves(board, Position, teamColor);
    }

    private List<ChessMove> GetKnightMoves(Board board)
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

    private List<ChessMove> GetBishopMoves(Board board)
    {
        return Bishop.GetDiagonalMoves(board, Position, teamColor);
    }

    private List<ChessMove> GetQueenMoves(Board board)
    {
        List<ChessMove> bishopMoves = Bishop.GetDiagonalMoves(board, Position, teamColor);
        bishopMoves.AddRange(Rook.GetRookMoves(board, Position, teamColor));
        return bishopMoves;
    }

    private List<ChessMove> GetKingMoves(Board board)
    {
        List<ChessMove> moves = new List<ChessMove>();

        AddIfNoTeamate(moves, Position + Vector2Int.up, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.down, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.left, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.right, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.up + Vector2Int.right, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.up + Vector2Int.left, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.down + Vector2Int.left, board, teamColor);
        AddIfNoTeamate(moves, Position + Vector2Int.down + Vector2Int.right, board, teamColor);

        return moves;
    }
}
[Serializable]
public struct RankToMesh
{
    public PieceType rank;
    public Mesh mesh;
}
