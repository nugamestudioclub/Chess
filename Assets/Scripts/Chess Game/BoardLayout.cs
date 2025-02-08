using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [System.Serializable]
    public class BoardSquareSetup
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }

    // square = an occupied space on the board
    [SerializeField] private BoardSquareSetup[] boardSquares;

    // returns amount of squares occupied by a piece
    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }

    // returns coordinates of a particular square
    // (0, 0) is bottom left
    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range!");
            return new Vector2Int(-1, -1);
        }

        return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
    }

    // returns name of piece on a particular square
    public string GetSquarePieceNameAtIndex(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range!");
            return "";
        }

        return boardSquares[index].pieceType.ToString();
    }

    // returns color of piece on a particular square
    public TeamColor GetSquareTeamColorAtIndex(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range!");
            return TeamColor.Black;
        }

        return boardSquares[index].teamColor;
    }

    public BoardSquareSetup[] SetupArray => boardSquares;
}