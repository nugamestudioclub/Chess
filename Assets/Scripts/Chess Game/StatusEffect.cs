using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public StatusEffectSO Props { get; protected set; }
    protected int id;
    public int ID => id;

    public StatusEffect(StatusEffectSO props, int id)
    {
        Props = props;
        this.id = id;
    }
}

public abstract class StatusEffectSO : ScriptableObject
{
    // these values shouldn't be modified during run time bc modifications wont have consistent effects

    [SerializeField] private int sortingOrder = 0;

    [SerializeField] private int onUpdateOrder = -1; // if -1 or less, piece uses sorting order,

    [SerializeField]
    private int onGameUpdateOrder = -1; // to determine the order for applying status in a section of Piece,

    [SerializeField] private int modifyMovesOrder = -1; // other wise, uses this case specific value
    [SerializeField] private int onPieceMoveOrder = -1;

    //

    public int OnUpdateOrder => onUpdateOrder < 0 ? sortingOrder : onUpdateOrder;
    public int OnGameUpdateOrder => onGameUpdateOrder < 0 ? sortingOrder : onGameUpdateOrder;
    public int ModifyMovesOrder => modifyMovesOrder < 0 ? sortingOrder : modifyMovesOrder;
    public int OnPieceMoveOrder => onPieceMoveOrder < 0 ? sortingOrder : onPieceMoveOrder;

    public Texture2D effectTex;

    public virtual void OnUpdate(StatusEffect effect, Piece piece)
    {
        return;
    }

    public virtual void OnGameUpdate(StatusEffect effect, Piece piece)
    {
        return;
    }

    public virtual void ModifyMoves(StatusEffect effect, List<ChessMove> curMoves)
    {
        return;
    }

    public virtual void OnPieceMove(StatusEffect effect, Piece piece, ChessMove move)
    {
        return;
    }

    public abstract StatusEffect Gen(int id);
}

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Events/Statuses/Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public NewHireSO newHireSO;
}