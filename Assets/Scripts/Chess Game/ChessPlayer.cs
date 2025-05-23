using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChessPlayer : MonoBehaviour
{
    Board board;

    Vector2Int selectedLocation = new Vector2Int(-1, -1);
    bool selected = false;
    bool hovered = false;
    public Piece hoveredPiece;
    public Piece selectedPiece;
    [SerializeField] private PieceManager pieceManager;

    List<ChessMove> potentialMoves = new List<ChessMove>();

    TeamColor teamColor = TeamColor.White;

    public Vector2Int hoverPos;

    [HideInInspector] public bool canClick = false;


    public static ChessPlayer instance;

    public List<SquareSelector> squares = new List<SquareSelector>();

    [SerializeField] private Camera mainCam;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TeamTurnText.instance.SetTeamsTurnText(teamColor);

        board = Board.instance;
// thing
        mainCam = Camera.main;

        RandomGameEvent.InitializeRedditComments();
        RandomGameEvent.SetNextRandomEvent();
        ActiveEventUI.instance.SetActiveEventText(RandomGameEvent.redditComments[RandomGameEvent.nextRandomEventIndex].GetName(), RandomGameEvent.redditComments[RandomGameEvent.nextRandomEventIndex].GetDescription());
    }

    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            ClearTileIndicators();
            HandleMouse();
        }

        HoverMouse();
        if (this.hovered)
        {
            this.board.showPieceInfo = true;
        }
        else
        {
            this.board.showPieceInfo = false;
        }

        this.pieceManager.hoveredPiece = this.hoveredPiece;
    }

    private void HoverMouse()
    {
        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;


        // Create a ray from the mouse position

        Ray ray = mainCam.ScreenPointToRay(mousePosition);


        // Perform raycast

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Square")))
        {
            var selector = hit.collider.gameObject.GetComponent<SquareSelector>();

            if (selector == null)
            {
                foreach (Piece piece in board.Pieces)
                {
                    piece.hovered = false;
                }

                return;
            }

            Vector2Int hoverCoords = selector.position;
            this.hoverPos = hoverCoords;
            this.hoveredPiece = this.board.GetPiece(hoverCoords);
            if (hoveredPiece == null)
            {
                this.hovered = false;
                foreach (Piece piece in board.Pieces)
                {
                    if (piece != null)
                    {
                        piece.hovered = false;
                    }
                }

                return;
            }

            this.hovered = true;
            this.hoveredPiece.hovered = true;
            foreach (Piece piece in board.Pieces)
            {
                if (piece != null && piece != this.hoveredPiece)
                {
                    piece.hovered = false;
                }
            }
        }
        else
        {
            this.hovered = false;
            foreach (Piece piece in board.Pieces)
            {
                if (piece != null)
                {
                    piece.hovered = false;
                }
            }
        }
    }

    private void HandleMouse()
    {
        Debug.Log("mouse down");

        foreach (Piece piece in board.Pieces)
        {
            if (piece != null)
            {
                piece.selected = false;
            }
        }

        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;


        // Create a ray from the mouse position

        Ray ray = mainCam.ScreenPointToRay(mousePosition);


        // Perform raycast

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Square")))
        {
            var selector = hit.collider.gameObject.GetComponent<SquareSelector>();
            if (selector == null)
            {
                return;
            }

            if (selected == false)
            {
                Debug.Log("new selected");
                HandleNewSelecection(selector.position);
            }
            else
            {
                Debug.Log("selected");
                HandleCurrentlySelected(selector.position);
            }

            this.selectedPiece = this.board.GetPiece(selector.position);


            if (this.selectedPiece != null)
            {
                this.selectedPiece.selected = true;
            }

            foreach (Piece piece in board.Pieces)
            {
                if (piece != null && piece != this.selectedPiece)
                {
                    piece.selected = false;
                }
            }
        }
        else
        {
            ClearSelection();
        }

        if (selectedPiece != null)
        {
            foreach (var move in selectedPiece.GetPossibleMoves(board))
            {
                foreach (SquareSelector square in squares)
                {
                    foreach (var pos in move.pathSteps)
                    {
                        if (square.position == pos)
                        {
                            square.ToggleMoveTileIndicator(true);
                            if (board.HasPiece(pos))
                            {
                                square.ToggleCapturableTileIndicator(true);
                            }
                        }
                    }

                    if (square.position == move.destination)
                    {
                        square.ToggleMoveTileIndicator(true);
                        if (board.HasPiece(move.destination))
                        {
                            square.ToggleCapturableTileIndicator(true);
                        }
                    }
                }

                Debug.Log("destination " + move.destination);
                

            }
        }
    }
    private void HandleNewSelecection(Vector2Int pos)
    {
        Piece piece = board.GetPiece(pos);

        if (piece == null)
        {
            ClearSelection();

            return;
        }

        selected = true;
        selectedLocation = pos;

        potentialMoves = piece.GetPossibleMoves(board);
        // display potential moves
        Debug.Log("Handle new selection 253");
    }

    // move the piece
    private void HandleCurrentlySelected(Vector2Int pos)
    {
        Piece current = board.GetPiece(selectedLocation);
        Piece piece = board.GetPiece(pos);
        if (piece != null)
        {
            if (piece.teamColor == current.teamColor)
            {
                HandleNewSelecection(pos);
                return;
            }
        }

        if (current.teamColor != teamColor)
        {
            HandleNewSelecection(pos);
            return;
        }

        
        foreach (ChessMove move in potentialMoves)
        {
            if (move.destination == pos)
            {
                var sentMove = move;
                //board.SetPiecePosition(board.GetPieceID(selectedLocation), pos);

                var selPiece = board.GetPiece(selectedLocation);

                if (selPiece.statuses.Contains(RandomStatus.Spleef)) {
                    foreach (var sqr in ChessPlayer.instance.squares)
                    {
                        if (sqr.position == piece.Position)
                        {
                            sqr.gameObject.SetActive(false);
                        }
                    }
                }
                
                RandomGameEvent.CallRandomEvent(selPiece, RandomGameEvent.nextRandomEventIndex);
                board.SendMove(teamColor, board.GetPiece(selectedLocation), sentMove, this);

                RandomGameEvent.SetNextRandomEvent();
                ActiveEventUI.instance.SetActiveEventText(RandomGameEvent.redditComments[RandomGameEvent.nextRandomEventIndex].GetName(), RandomGameEvent.redditComments[RandomGameEvent.nextRandomEventIndex].GetDescription());

                //RandomGameEvent.GetRandomStatus();
                
                this.selectedPiece = null;

                ClearSelection();

                return;
            }
        }
        ClearSelection();
    }

    private void ClearSelection()
    {
        selected = false;
        if (this.selectedPiece != null)
        {
            this.selectedPiece.selected = false;
            this.selectedPiece = null;
        }

        potentialMoves.Clear();

        selectedLocation = new Vector2Int(-1, -1);
    }

    public void Switch()
    {
        teamColor = teamColor == TeamColor.White ? TeamColor.Black : TeamColor.White;
        TeamTurnText.instance.SetTeamsTurnText(teamColor);
    }

    public void ClearTileIndicators()
    {
        foreach (SquareSelector square in squares)
        {
            square.ToggleMoveTileIndicator(false);
            square.ToggleCapturableTileIndicator(false);
        }
    }
}