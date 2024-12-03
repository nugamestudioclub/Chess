using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private ChessPieceDialogueBox infoBox;
    [SerializeField] private TMP_Text pieceInfoText;
    public Piece hoveredPiece;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string displayText = this.hoveredPiece.rank + " (" + this.hoveredPiece.teamColor + ")\n\n"
            + "Status Effects:\n";
        foreach (StatusEffect statusEffect in this.hoveredPiece.statuses)
        {
            displayText = displayText + statusEffect.ToString() + "\n";
        }
        this.pieceInfoText.SetText(displayText);
    }
}
