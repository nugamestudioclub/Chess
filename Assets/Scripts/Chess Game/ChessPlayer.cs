using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ChessPlayer : MonoBehaviour
{
    Board board;

    Vector2Int selectedLocation;
    bool selected = false;
    bool hovered = false;
    public Piece hoveredPiece;
    public Piece selectedPiece;

    List<ChessMove> potentialMoves;

    TeamColor teamColor = TeamColor.White;

    public Vector2Int hoverPos;

    private void Awake()
    {
        selectedLocation = new Vector2Int(-1, -1);
        potentialMoves = new List<ChessMove>();
        selected = false;

        board = FindObjectOfType<Board>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
    }

    private void HoverMouse()
    {

        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;


        // Create a ray from the mouse position

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);


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
                    piece.hovered = false;
                }
                return;
            }
            this.hovered = true;
            this.hoveredPiece.hovered = true;
            foreach (Piece piece in board.Pieces)
            {
                if (piece != this.hoveredPiece)
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
                piece.hovered = false;
            }
        }
    }

    private void HandleMouse()
    {
        Debug.Log("mouse down");

        foreach (Piece piece in board.Pieces)
        {
            piece.selected = false;
        }

        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;


        // Create a ray from the mouse position

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);


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
            this.selectedPiece.selected = true;
            foreach (Piece piece in board.Pieces)
            {
                if (piece != this.selectedPiece)
                {
                    piece.selected = false;
                }
            }
        }
        else
        {
            ClearSelection();
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
    }

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

                //board.SetPiecePosition(board.GetPieceID(selectedLocation), pos);

                board.SendMove(teamColor, board.GetPiece(selectedLocation), move, this);

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
    }
}
