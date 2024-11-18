using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    const int EMPTY_SPACE = -1;

    const int TILE_TO_VECT = 2;

    [SerializeField] private PieceCreator pieceCreator;
    [SerializeField] private BoardLayout layout;

    [SerializeField] public int Width;
    [SerializeField] public int Height;
    private List<int> pieceIDGrid;
    private List<Piece> pieces;

    public void Start()
    {
        StartGame();
    }

    #region gameloop

    TeamColor activePlayer = TeamColor.White;
    bool gameOver = false;

    public void StartGame()
    {
        pieceIDGrid = Enumerable.Repeat(EMPTY_SPACE, Width * Height).ToList();
        pieces = new List<Piece>();

        foreach (BoardLayout.BoardSquareSetup setup in layout.SetupArray)
        {
            AddPiece(setup.pieceType, setup.position - new Vector2Int(1, 1), setup.teamColor);
        }
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

    public bool AddPiece(PieceType pt, Vector2Int position, TeamColor tc)
    {
        if (HasPiece(position)) return false;

        GameObject pieceObject = pieceCreator.CreatePiece(pt, transform, tc);
        Piece piece = pieceObject.GetComponent<Piece>();
        piece.SetPosition(position);
        Debug.Log(position);
        piece.transform.position = PosToVect(position);
        pieces.Add(piece);
        
        SetPieceGridID(pieces.Count - 1, position);

        return true;
    }

    public Vector3 PosToVect(Vector2Int pos)
    {
        Vector3 ret = new Vector3(pos.x * 2.55f - 8.8f, 1.8f, pos.y * 2.55f - 9.5f);
        return ret;
    }

    public void SetPiecePosition(int id, Vector2Int position)
    {
        if (HasPiece(position)) RemovePiece(position);

        SetPieceGridID(EMPTY_SPACE, pieces[id].Position);
        SetPieceGridID(id, position);
        pieces[id].SetPosition(position);
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

    public int GetPieceID(Vector2Int position)
    {
        return pieceIDGrid[GetFlat(position)];
    }

    public int GetFlat(Vector2Int position) => position.x + (position.y * Width);

    public Vector2Int Get2D(int location) => new Vector2Int(location % Width, location / Width);

    public bool ContainsPosition(Vector2Int position)
    {
        if (position.x < 0 || position.x >= Width)
        {
            return false;
        }

        if (position.y < 0 || position.y >= Height)
        {
            return false;
        }

        return true;
    }
}
