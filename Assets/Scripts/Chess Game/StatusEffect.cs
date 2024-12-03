using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    // these values shouldn't be modified during run time bc modifications wont have consistent effects

    [SerializeField] private int sortingOrder = 0;

    [SerializeField] private int updateOrder = -1;      // if -1 or less, piece uses sorting order,
    [SerializeField] private int gameUpdateOrder = -1;  // to determine the order for applying status in a section of Piece,
    [SerializeField] private int modifyMovesOrder = -1; // other wise, uses this case specific value

    //

    public int UpdateOrder => updateOrder < 0 ? sortingOrder : updateOrder;
    public int GameUpdateOrder => gameUpdateOrder < 0 ? sortingOrder : gameUpdateOrder;
    public int ModifyMovesOrder => modifyMovesOrder < 0 ? sortingOrder : modifyMovesOrder;

    public Texture2D effectTex;

    public abstract void OnPieceMove(Piece piece, ChessMove move);
    public abstract void OnUpdate(Piece piece);
    public abstract void OnGameUpdate(Piece piece);
    public abstract List<ChessMove> ModifyMoves(List<ChessMove> curMoves);
}
