using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer : MonoBehaviour
{
    Board board;

    Vector2Int selectedLocation;
    Piece hoveredPiece;
    Piece selectedPiece;
    public Vector2Int hoverPos;
    bool selected = false;

    List<ChessMove> potentialMoves;

    TeamColor teamColor = TeamColor.White;

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
                return;
            }

            Vector2Int hoverCoords = selector.position;
            this.hoveredPiece = this.board.GetPiece(hoverCoords);
        }
    }

    private void HandleMouse()
    {
        Debug.Log("mouse down");

        // Get mouse position
        Vector3 mousePosition = Input.mousePosition;


        // Create a ray from the mouse position

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);


        // Perform raycast

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Square")))
        {
            Debug.Log("raycast hit");

            var selector = hit.collider.gameObject.GetComponent<SquareSelector>();
            if (selector == null)
            {
                return;
            }

            if (selected == false)
            {
                Debug.Log("new selected");
                //HandleNewSelecection(selector.position);
            } 
            else
            {
                Debug.Log("selected");
                //HandleCurrentlySelected(selector.position);
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

                ClearSelection();

                return;
            }
        }

        ClearSelection();
    }

    private void ClearSelection()
    {
        selected = false;
        potentialMoves.Clear();
        selectedLocation = new Vector2Int(-1, -1);
    }

    public void Switch()
    {
        teamColor = teamColor == TeamColor.White ? TeamColor.Black : TeamColor.White;
    }
}
