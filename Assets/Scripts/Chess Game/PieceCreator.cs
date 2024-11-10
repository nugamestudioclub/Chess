using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Piece Creator")]
public class PieceCreator : ScriptableObject
{
    [SerializeField] private GameObject pawn;
    [SerializeField] private GameObject knight;
    [SerializeField] private GameObject bishop;
    [SerializeField] private GameObject rook;
    [SerializeField] private GameObject queen;
    [SerializeField] private GameObject king;
    public GameObject createPiece(PieceType pt, Transform parent)
    {
        GameObject pieceToSpawn;

        switch (pt)
        {
            case PieceType.Pawn:
                pieceToSpawn = pawn;
                break;
            case PieceType.Bishop:
                pieceToSpawn = bishop;
                break;
            case PieceType.Knight:
                pieceToSpawn = knight;
                break;
            case PieceType.Rook:
                pieceToSpawn = rook;
                break;
            case PieceType.Queen:
                pieceToSpawn = queen;
                break;
            case PieceType.King:
                pieceToSpawn = king;
                break;
            default:
                pieceToSpawn = pawn;
                break;
        }

        return Instantiate(pieceToSpawn, parent);
    }
}
