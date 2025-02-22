using System;
using System.Collections.Generic;

public class StatusAcceptor
{
    private int cur = 0;
    public Dictionary<int, StatusEffect> Stati { get; private set; }

    public List<StatusEffect> ToList()
    {
        List<StatusEffect> res = new();

        foreach (var effect in Stati.Values)
        {
            res.Add(effect);
        }

        return res;
    }

    private StatusApplicationList onPieceMoveAppList;
    private StatusApplicationList onUpdateAppList;
    private StatusApplicationList onGameUpdateAppList;
    private StatusApplicationList modifyMoveAppList;

    public StatusAcceptor()
    {
        Stati = new();

        onPieceMoveAppList = new StatusApplicationList(
            (effect) => effect.Props.OnPieceMoveOrder, this);
        onUpdateAppList = new StatusApplicationList(
            (effect) => effect.Props.OnUpdateOrder, this);
        onGameUpdateAppList = new StatusApplicationList(
            (effect) => effect.Props.OnGameUpdateOrder, this);
        modifyMoveAppList = new StatusApplicationList(
            (effect) => effect.Props.ModifyMovesOrder, this);
    }

    public StatusEffect AcceptStatus(StatusEffectSO statusSO)
    {
        var status = statusSO.Gen(cur);
        Stati.Add(cur, status);

        onPieceMoveAppList.AcceptStatus(cur, status);
        onUpdateAppList.AcceptStatus(cur, status);
        onGameUpdateAppList.AcceptStatus(cur, status);
        modifyMoveAppList.AcceptStatus(cur, status);

        cur++;

        return status;
    }

    public void RemoveStatus(int id)
    {
        Stati.Remove(id);
        onPieceMoveAppList.RemoveStatus(id);
        onUpdateAppList.RemoveStatus(id);
        onGameUpdateAppList.RemoveStatus(id);
        modifyMoveAppList.RemoveStatus(id);
    }

    public bool HasStatusType<EffectType>() where EffectType : StatusEffect
    {
        foreach (var effect in Stati.Values)
        {
            if (effect.GetType() == typeof(EffectType))
            {
                return true;
            }
        }

        return false;
    }

    public void ApplyOnPieceMove(Action<StatusEffect> applier) => onPieceMoveAppList.ApplyStatus(applier);
    public void ApplyOnUpdate(Action<StatusEffect> applier) => onUpdateAppList.ApplyStatus(applier);
    public void ApplyOnGameUpdate(Action<StatusEffect> applier) => onGameUpdateAppList.ApplyStatus(applier);
    public void ApplyModifyMoves(Action<StatusEffect> applier) => modifyMoveAppList.ApplyStatus(applier);
}

class StatusApplicationList
{
    private readonly List<int> idPositions;
    private readonly Func<StatusEffect, int> orderGetter;
    private readonly StatusAcceptor acceptor;

    public StatusApplicationList(Func<StatusEffect, int> orderGetter, StatusAcceptor acceptor)
    {
        idPositions = new();
        this.orderGetter = orderGetter;
        this.acceptor = acceptor;
    }

    public void AcceptStatus(int id, StatusEffect status)
    {
        int order = orderGetter.Invoke(status);

        var head = 0;
        for (; head < idPositions.Count && orderGetter.Invoke(acceptor.Stati[idPositions[head]]) < order; head++) ;

        idPositions.Insert(head, id);
    }

    public void RemoveStatus(int id)
    {
        idPositions.Remove(id);
    }

    public void ApplyStatus(Action<StatusEffect> applier)
    {
        var idPClone = new List<int>();

        for (int i = 0; i < idPositions.Count; i++)
        {
            idPClone.Add(idPositions[i]);
        }

        for (int i = 0; i < idPClone.Count; i++)
        {
            var id = idPClone[i];

            if (!idPositions.Contains(id))
            {
                continue;
            }

            applier.Invoke(acceptor.Stati[id]);
        }
    }
}