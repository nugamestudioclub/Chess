using System;
using TMPro;
using UnityEngine;

public class TeamTurnText : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI teamsTurnText;

    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;

    public static TeamTurnText instance;

    private void Awake()
    {
        instance = this;
    }
//
    public void SetTeamsTurnText(TeamColor team)
    {
        teamsTurnText.text = team + "'s " + "Turn";

        switch (team)
        {
            case TeamColor.White:
                teamsTurnText.color = whiteMaterial.color;
                break;
            case TeamColor.Black:
                teamsTurnText.color = blackMaterial.color;
                break;
        }
    }
}
