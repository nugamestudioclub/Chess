using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private ChessPieceDialogueBox infoBox;
    [SerializeField] private TMP_Text pieceInfoText;
    [SerializeField] private TMP_Text pieceIcon;

    public Piece hoveredPiece;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hoveredPiece == null)
        {
            return;
        }

        string displayText = this.hoveredPiece.rank + " (" + this.hoveredPiece.teamColor + ")\n\n"
                             + "Status Effects:\n";
        foreach (StatusEffect statusEffect in this.hoveredPiece.statuses)
        {
            displayText = displayText + statusEffect.ToString() + "\n";
        }

        pieceIcon.text = "";

        string pieceIconFont = "";
        
        if(this.hoveredPiece.teamColor == TeamColor.Black)
        {
            switch (this.hoveredPiece.rank)
            {
                case "King":
                    pieceIconFont = "k";                
                    break;
                case "Queen":
                    pieceIconFont = "q";               
                    break;
                case "Rook":
                    pieceIconFont = "r"; 
                    break;
                case "Bishop":
                    pieceIconFont = "b"; 
                    break;
                case "Knight":
                    pieceIconFont = "h"; 
                    break;
                case "Pawn":
                    pieceIconFont = "p";               
                    break;
            }
        }
        else if(this.hoveredPiece.teamColor == TeamColor.White)
        {
               switch (this.hoveredPiece.rank)
            {
                case "King":
                    pieceIconFont = "l";                
                    break;
                case "Queen":
                    pieceIconFont = "w";                
                    break;
                case "Rook":
                    pieceIconFont = "t"; 
                    break;
                case "Bishop":
                    pieceIconFont = "n"; 
                    break;
                case "Knight":
                    pieceIconFont = "j"; 
                    break;
                case "Pawn":
                    pieceIconFont = "o";               
                    break;
            }
        }
        pieceIcon.text += pieceIconFont;

        this.pieceInfoText.SetText(displayText);
    }
}