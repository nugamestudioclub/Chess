using UnityEngine;

public class RUOK : ARedditComment
{
    public override string GetName()
    {
        return "Are you feeling alright?";
    }

    public override string GetDescription()
    {
        return "Visuals are distorted for 1 turn";
    }

    public override int GetDuration()
    {
        return -1;
    }
    public override void SaySomeDumbShit(Piece piece)
    {
        GlobalURPVolumeManager.Instance.SetProfileToTrippy();

    }
}
