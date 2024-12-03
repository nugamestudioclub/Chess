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
        }
    }
}


public class NewHire : StatusEffect
{
    public int numTurns = 6;

    public NewHire(StatusEffectSO nhso, int id) : base(nhso, id) { }
}