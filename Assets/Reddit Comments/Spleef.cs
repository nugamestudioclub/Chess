using UnityEngine;

public class Spleef : ARedditComment
{

    public int duration = 1; 

    public override string GetName()
    {
        return "Spleef";
    }

    public override string GetDescription()
    {
        return "The tile you just moved from falls into the void. Lasts 3 turns";
    }

    public override int GetDuration()
    {
        duration--;
        return duration;
    }

    public override void SaySomeDumbShit(Piece piece)
    {
        foreach (var sqr in ChessPlayer.instance.squares)
        {
            if (sqr.position == piece.Position)
            {
                sqr.spleefParticles.name = "SPLEEF: " + sqr.spleefParticles.transform.parent.name;
                sqr.spleefParticles.transform.parent = null;
                sqr.spleefParticles.GetComponent<ParticleSystemRenderer>().material = sqr.tileMeshRenderer.material;
                sqr.spleefParticles.Play();
                sqr.gameObject.SetActive(false);
            }
        }
        piece.spleefParticles.Play();
        piece.status = RandomStatus.Spleef;
    }
}
