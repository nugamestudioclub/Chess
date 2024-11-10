using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    const int EMPTY_SPACE = -1;

    [SerializeField] private PieceCreator pieceCreator;

    [SerializeField] public int Width { get; private set; }
    [SerializeField] public int Height { get; private set; }

    private List<int> pieceIDGrid;
    private List<Piece> pieces;

    public void Start()
    {
        
    }

    #region gameloop

    TeamColor activePlayer = TeamColor.White;
    bool gameOver = false;

    public void StartGame()
    {
        pieceIDGrid = Enumerable.Repeat(EMPTY_SPACE, Width * Height).ToList();
    }

    public void UpdateGame()
    {
        foreach (Piece piece in pieces)
        {
            piece.GameUpdate();
        }
    }

    public bool SendMove(TeamColor tc, Piece piece, ChessMove move)
    {
        if (tc != activePlayer || gameOver) return false;

        SetPiecePosition(GetPieceID(piece.Position), move.destination);

        // if piece captured king

        // else:

        activePlayer = activePlayer == TeamColor.White ? TeamColor.Black : TeamColor.White;
        UpdateGame();

        return true;
    }

    #endregion

    public bool AddPiece(PieceType pt, Vector2Int position)
    {
        if (HasPiece(position)) return false;

        GameObject pieceObject = pieceCreator.createPiece(pt, transform);
        Piece piece = pieceObject.GetComponent<Piece>();
        piece.setPosition(position);
        pieces.Add(piece);
        
        SetPieceGridID(pieces.Count - 1, position);

        return true;
    }

    public void SetPiecePosition(int id, Vector2Int position)
    {
        if (HasPiece(position)) RemovePiece(position);

        SetPieceGridID(EMPTY_SPACE, pieces[id].Position);
        SetPieceGridID(id, position);
        pieces[id].setPosition(position);
    }

    public void RemovePiece(Vector2Int position)
    {
        int id = GetPieceID(position);
        pieces[id] = null;
        Destroy(pieces[id].gameObject);
    }

    public Piece GetPiece(Vector2Int position)
    {
        int id = GetPieceID(position);
        return id == EMPTY_SPACE ? null : pieces[id];
    }

    public bool HasPiece(Vector2Int position) => GetPieceID(position) != EMPTY_SPACE;

    private void SetPieceGridID(int id, Vector2Int position) => pieceIDGrid[GetFlat(position)] = id;

    public int GetPieceID(Vector2Int position) => pieceIDGrid[GetFlat(position)];

    public int GetFlat(Vector2Int position) => position.x + (position.y * Width);
}
