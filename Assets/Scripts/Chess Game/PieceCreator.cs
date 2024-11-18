using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Piece Creator")]
public class PieceCreator : ScriptableObject
{
    [SerializeField] private GameObject bPawn;
    [SerializeField] private GameObject bKnight;
    [SerializeField] private GameObject bBishop;
    [SerializeField] private GameObject bRook;
    [SerializeField] private GameObject bQueen;
    [SerializeField] private GameObject bKing;

    [SerializeField] private GameObject wPawn;
    [SerializeField] private GameObject wKnight;
    [SerializeField] private GameObject wBishop;
    [SerializeField] private GameObject wRook;
    [SerializeField] private GameObject wQueen;
    [SerializeField] private GameObject wKing;
    public GameObject CreatePiece(PieceType pt, Transform parent, TeamColor tc)
    {
        GameObject pieceToSpawn;

        switch (pt)
        {
            case PieceType.Pawn:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bPawn;
                }
                else
                {
                    pieceToSpawn = wPawn;
                }
                break;
            case PieceType.Bishop:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bBishop;
                }
                else
                {
                    pieceToSpawn = wBishop;
                }
                break;
            case PieceType.Knight:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bKnight;
                }
                else
                {
                    pieceToSpawn = wKnight;
                }
                break;
            case PieceType.Rook:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bRook;
                }
                else
                {
                    pieceToSpawn = wRook;
                }
                break;
            case PieceType.Queen:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bQueen;
                }
                else
                {
                    pieceToSpawn = wQueen;
                }
                break;
            case PieceType.King:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bKing;
                }
                else
                {
                    pieceToSpawn = wKing;
                }
                break;
            default:
                if (tc == TeamColor.Black)
                {
                    pieceToSpawn = bPawn;
                }
                else
                {
                    pieceToSpawn = wPawn;
                }
                break;
        }

        return Instantiate(pieceToSpawn, parent);
    }
}
