using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GamePiece : Piece
{
    public PieceType pieceType = PieceType.Pawn;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private Material whitePieceMaterial;
    [SerializeField] private Material blackPieceMaterial;

    public List<RankToMesh> rankToMeshes = new List<RankToMesh>();
    
    private void OnEnable()
    {
        UpdateVisual();
    }

    private void OnValidate()
    {
UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter is not assigned on " + gameObject.name);
            return;
        }
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer is not assigned on " + gameObject.name);
            return;
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

        switch (teamColor)
        {
            case TeamColor.White:
                meshRenderer.material = whitePieceMaterial;
                break;
            case TeamColor.Black:
                meshRenderer.material = blackPieceMaterial;
                break;
        }
    }
    
    public override List<ChessMove> GetDefaultMoves(Board board)
    {
        var pieceMoves = Bishop.GetDiagonalMoves(board, Position, teamColor);
        pieceMoves.AddRange(Rook.GetRookMoves(board, Position, teamColor));

        return pieceMoves;
    }
}

[Serializable]
public struct RankToMesh
{
    public PieceType rank;
    public Mesh mesh;
}