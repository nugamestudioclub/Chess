using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [Header("Piece Info")]
    public PieceType pieceType = PieceType.Pawn;
    [HideInInspector] public Vector2Int Position { get; private set; }

    public TeamColor teamColor;

    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool selected = false;
    [HideInInspector] public bool hovered = false;

    [Header("Particles")]
    public ParticleSystem effectParticles;
    public ParticleSystem warpParticles;
    public ParticleSystem convertParticles;
    public ParticleSystem spleefParticles;
    public ParticleSystem nullParticles;
    public ParticleSystem blessedParticles;
    public ParticleSystem killParticles;

    
    [Header("Mesh Renderer")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [Header("Piece Materials")]
    [SerializeField] private Material whitePieceMaterial;
    [SerializeField] private Material blackPieceMaterial;

    public List<RankToMesh> rankToMeshes = new List<RankToMesh>();

    
    // public List<StatusEffect> statuses => statusManager.ToList();

    [HideInInspector] public RandomStatus status = RandomStatus.None;
    [HideInInspector] public List<RandomStatus> statuses = new List<RandomStatus>();

    public void SetPosition(Vector2Int position, Vector3 worldPos)
    {
        Position = position;
        transform.position = worldPos;
    }

    public virtual List<ChessMove> GetPossibleMoves(Board board)
    {
        var moves = GetDefaultMoves(board);


        return moves;
    }


    private void Update()
    {
        Debug.Log("Rotation: " + transform.rotation + " name: " + gameObject.name);
        Renderer meshRenderer = GetComponent<Renderer>();
        if (meshRenderer != null)
        {
            if (this.hovered)
            {
                meshRenderer.materials[1].SetFloat("_Alpha", 1.0f);
            }
            else
            {
                meshRenderer.materials[1].SetFloat("_Alpha", 0.0f);
            }

            if (this.selected)
            {
                meshRenderer.materials[1].SetFloat("_Alpha", 1.0f);
                meshRenderer.materials[1].SetColor("_Color", new Color(0, 255, 0));
            }
            else
            {
                meshRenderer.materials[1].SetColor("_Color", new Color(255, 255, 255));
            }
        }

    }

    public static void AppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board,
        TeamColor teamColor)
    {
        DoAppendLine(pos, diagonal, step, moves, board, new List<Vector2Int>(), pos, teamColor);
    }

    public static void DoAppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board,
        List<Vector2Int> path, Vector2Int origin, TeamColor teamColor)
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

    private void OnEnable()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        switch (teamColor)
        {
            case TeamColor.White:
                meshRenderer.material = whitePieceMaterial;
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                Debug.Log("RESET TRANSFORM 000");

                break;
            case TeamColor.Black:
                meshRenderer.material = blackPieceMaterial;
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                Debug.Log("RESET TRANSFORM 180");

                break;
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
    }

    public virtual List<ChessMove> GetDefaultMoves(Board board)
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