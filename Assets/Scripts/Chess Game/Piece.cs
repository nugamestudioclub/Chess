using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    
    public PieceType pieceType = PieceType.Pawn;
    public Vector2Int Position { get; private set; }

    public TeamColor teamColor;
    
    public bool hasMoved = false;
    public bool selected = false;
    public bool hovered = false;

    public StatusAcceptor statusManager = new();
    public List<StatusEffect> statuses => statusManager.ToList();
    
    
    public void SetPosition(Vector2Int position, Vector3 worldPos)
    {
        Position = position;
        transform.position = worldPos;
    }

    public virtual List<ChessMove> GetPossibleMoves(Board board)
    {
        var moves = GetDefaultMoves(board);

        statusManager.ApplyModifyMoves((effect) => effect.Props.ModifyMoves(effect, moves));

        return moves;
    }

    public abstract List<ChessMove> GetDefaultMoves(Board board);

    public virtual void GameUpdate()
    {
        statusManager.ApplyOnGameUpdate((effect) => effect.Props.OnGameUpdate(effect, this));
        return;
    }

    private void Update()
    {
        if (this.hovered)
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f);
        }
        else
        {
            this.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        }

        if (this.selected)
        {
            var hoveredPieceMat = this.GetComponent<Renderer>().material;
            hoveredPieceMat.SetFloat("_Alpha", 1.0f);
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 255, 0));
        }
        else
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 255, 255));
        }


        statusManager.ApplyOnUpdate((effect) => effect.Props.OnUpdate(effect, this));
    }

    public static void AppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board,
        TeamColor teamColor)
    {
        DoAppendLine(pos, diagonal, step, moves, board, new List<Vector2Int>(), pos, teamColor);
    }

    public static void DoAppendLine(Vector2Int pos, Vector2Int diagonal, int step, List<ChessMove> moves, Board board,
        List<Vector2Int> path, Vector2Int origin, TeamColor teamColor)
    {
        Vector2Int checkPos = pos + (diagonal * step);

        if (!board.ContainsPosition(checkPos))
        {
            return;
        }

        Piece piece = board.GetPiece(checkPos);

        if (piece != null)
        {
            if (piece.teamColor == teamColor)
            {
                return;
            }
        }

        ChessMove move = new ChessMove()
        {
            origin = new Vector2Int(origin.x, origin.y),
            destination = new Vector2Int(checkPos.x, checkPos.y),
            pathSteps = new List<Vector2Int>(path)
        };

        moves.Add(move);

        path.Add(checkPos);

        if (piece == null)
        {
            DoAppendLine(checkPos, diagonal, step, moves, board, path, origin, teamColor);
        }
    }

    protected void AddIfNoTeamate(List<ChessMove> moves, Vector2Int position, Board board, TeamColor teamColor)
    {
        if (!board.ContainsPosition(position))
        {
            return;
        }

        Piece piece = board.GetPiece(position);

        if (piece != null)
        {
            if (piece.teamColor == teamColor)
            {
                return;
            }
        }

        ChessMove move = new ChessMove()
        {
            destination = new Vector2Int(position.x, position.y),
            origin = new Vector2Int(Position.x, Position.y),
            pathSteps = new List<Vector2Int>()
        };

        moves.Add(move);
    }
}

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