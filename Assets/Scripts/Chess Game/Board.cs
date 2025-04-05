using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    const int EMPTY_SPACE = -1;

    [SerializeField] private BoardLayout layout;
    [SerializeField] private SquareSelectorCreator squareCreator;

    [SerializeField] private GameObject piecePrefab;
    public bool showPieceInfo = false;

    public void SetActivePlayer(TeamColor value)
    {
        activePlayer = value;
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    [SerializeField] public int Width;
    [SerializeField] public int Height;
    private List<int> pieceIDGrid;

    private List<Piece> pieces;
    public List<Piece> Pieces => pieces;

    public static Board instance;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        eventManager.board = this;
        StartGame();
    }

    #region gameloop

    private int numMoves = 0;

    public int NumMoves
    {
        get => numMoves;
        set => numMoves = value;
    }

    ChessController mainController;
    ChessController activeController;

    public EventManager eventManager;

    public TeamColor activePlayer = TeamColor.White;
    public bool gameOver = false;

    public void StartGame()
    {
        pieceIDGrid = Enumerable.Repeat(EMPTY_SPACE, Width * Height).ToList();
        pieces = new List<Piece>();

        float delayBetweenSpawns = 0.01f;
        
        squareCreator.CreateSquareSelectors(this, delayBetweenSpawns);

        
        mainController = new ChessController(this);
        activeController = mainController;
    }

    public IEnumerator SpawnPieces(float delayBetweenSpawns)
    {
        foreach (BoardLayout.BoardSquareSetup setup in layout.SetupArray)
        {
            AddPiece(setup.pieceType, setup.position - new Vector2Int(1, 1), setup.teamColor);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
    
    public void UpdateGame()
    {
        activeController.UpdateGame();
    }

    // returns whether or not move was made
    public bool SendMove(TeamColor tc, Piece piece, ChessMove move, ChessPlayer sender = null)
    {
        return activeController.SendMove(tc, piece, move, sender);
    }

    private void SetController(ChessController controller)
    {
        activeController.OnEnd();
        activeController = controller;
        activeController.OnStart();
    }

    #endregion

    public bool AddPiece(PieceType pt, Vector2Int position, TeamColor tc)
    {
        if (HasPiece(position)) return false;

        GameObject pieceObject = Instantiate(piecePrefab, transform);

        GamePiece pieceComponent = pieceObject.GetComponent<GamePiece>();


        pieceComponent.pieceType = pt;
        pieceComponent.teamColor = tc;
        pieceComponent.UpdateVisual();
        pieceObject.name = tc + " " + pt;

        Piece piece = pieceObject.GetComponent<Piece>();
        piece.SetPosition(position, PosToVect(position));
        piece.teamColor = tc;
        Debug.Log(position);
        pieces.Add(piece);

        SetPieceGridID(pieces.Count - 1, position);

        return true;
    }

    public Vector3 PosToVect(Vector2Int pos)
    {
        Vector3 ret = new Vector3(pos.x * 2.55f - 8.75f, 1.8f, pos.y * 2.55f - 9.5f); // what are these numbers
        return ret;
    }

    public void SetPiecePosition(int id, Vector2Int position)
    {
        if (HasPiece(position)) RemovePiece(position);

        SetPieceGridID(EMPTY_SPACE, pieces[id].Position);
        SetPieceGridID(id, position);

        Vector3 temp = transform.eulerAngles + Vector3.zero;
        transform.eulerAngles = new Vector3(0, 0, 0);
        pieces[id].SetPosition(position, PosToVect(position));
        transform.eulerAngles = temp;
    }

    public void RemovePiece(Vector2Int position)
    {
        int id = GetPieceID(position);
        if (id == EMPTY_SPACE || id < 0 || id >= pieces.Count)
        {
            Debug.LogWarning($"Tried to remove a piece at {position}, but no valid piece found.");
            return;
        }

        if (pieces[id] != null && pieces[id].gameObject != null)
        {
            Destroy(pieces[id].gameObject);
            pieces[id] = null;
        }
        else
        {
            Debug.LogWarning("Tried to remove a null piece");
        }
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
        if (!ContainsPosition(position))
        {
            Debug.LogWarning($"GetPieceID: Position {position} is out of bounds.");
            return EMPTY_SPACE; // Return -1 to indicate an invalid piece
        }

        int index = GetFlat(position);
    
        if (index < 0 || index >= pieceIDGrid.Count)
        {
            Debug.LogError($"GetPieceID: Computed index {index} is out of range.");
            return EMPTY_SPACE;
        }

        return pieceIDGrid[index];
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

    public IEnumerator FlipBoard()
    {
        float initialYRot = transform.eulerAngles.y;
        float offsetY = 0;
        float offsetControl = 0;

        while (offsetControl <= 1)
        {
            offsetControl += Time.deltaTime;
            offsetY = EaseInOutQuint(offsetControl) * 180;
            transform.eulerAngles = new Vector3(transform.rotation.x, initialYRot + offsetY, transform.rotation.z);


            ChessPlayer.instance.ClearTileIndicators();
            foreach (Piece piece in pieces)
            {
                if (piece != null)
                {
                    piece.hovered = false;
                    piece.selected = false;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(transform.rotation.x, initialYRot + 180f, transform.rotation.z);
    }

    float EaseInOutQuint(float x)
    {
        float c5 = (2 * Mathf.PI) / 4.5f;

        return x == 0
            ? 0
            : x == 1
                ? 1
                : x < 0.5
                    ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
                    : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    }
}