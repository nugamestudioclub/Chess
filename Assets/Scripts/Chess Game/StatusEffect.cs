using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public StatusEffectSO Props { get; private set; }

    public StatusEffect(StatusEffectSO props)
    {
        Props = props;
    }
}

public abstract class StatusEffectSO: ScriptableObject
{
    // these values shouldn't be modified during run time bc modifications wont have consistent effects

    [SerializeField] private int sortingOrder = 0;

    [SerializeField] private int onUpdateOrder = -1;      // if -1 or less, piece uses sorting order,
    [SerializeField] private int onGameUpdateOrder = -1;  // to determine the order for applying status in a section of Piece,
    [SerializeField] private int modifyMovesOrder = -1; // other wise, uses this case specific value
    [SerializeField] private int onPieceMoveOrder = -1;

    //

    public int OnUpdateOrder => onUpdateOrder < 0 ? sortingOrder : onUpdateOrder;
    public int OnGameUpdateOrder => onGameUpdateOrder < 0 ? sortingOrder : onGameUpdateOrder;
    public int ModifyMovesOrder => modifyMovesOrder < 0 ? sortingOrder : modifyMovesOrder;
    public int OnPieceMoveOrder => onPieceMoveOrder < 0 ? sortingOrder : onPieceMoveOrder;

    public Texture2D effectTex;

    public abstract void OnUpdate(Piece piece);
    public abstract void OnGameUpdate(Piece piece);
    public abstract void ModifyMoves(List<ChessMove> curMoves);
    public abstract void OnPieceMove(Piece piece, ChessMove move);

    public abstract StatusEffect Gen();
}
