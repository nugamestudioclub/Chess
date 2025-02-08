using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHire", menuName = "Events/Statuses/New Hire")]
public class NewHireSO : StatusEffectSO
{
    public override StatusEffect Gen(int id)
    {
        return new NewHire(this, id);
    }

    public override void OnGameUpdate(StatusEffect effect, Piece piece)
    {
        var nh = effect as NewHire;
        nh.numTurns--;

        if (nh.numTurns <= 0)
        {
            piece.statusManager.RemoveStatus(effect.ID);
            nh?.nhe.End();
        }
    }
}

public class NewHire : StatusEffect
{
    public int numTurns = 6;
    public NewHireEvent nhe = null;

    public NewHire(StatusEffectSO nhso, int id) : base(nhso, id)
    {
    }
}

[CreateAssetMenu(fileName = "NewHireEvent", menuName = "Events/New Hire Event")]
public class NewHireEventSO : LinkRandomSO<NewHireEvent>
{
}

public class NewHireEvent : RandomEvent
{
    private NewHire status;

    protected override void DoEnd()
    {
        return;
    }

    protected override void DoStart()
    {
        var candidatePieces = new List<Piece>();

        foreach (var piece in board.Pieces)
        {
            if (piece.teamColor == this.team)
            {
                candidatePieces.Add(piece);
            }
        }

        var rand = new System.Random();

        if (candidatePieces.Count < 1)
        {
            End();
            return;
        }

        int sel = rand.Next(candidatePieces.Count);
        var selected = candidatePieces[sel];

        Debug.Log("new hire" + selected.Position.x + selected.Position.y + selected.rank);
        status = selected.statusManager.AcceptStatus(manager.statusData.newHireSO) as NewHire;
        status.nhe = this;
    }
}