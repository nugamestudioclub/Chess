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

        string displayText = this.hoveredPiece.pieceType + " (" + this.hoveredPiece.teamColor + ")\n\n"
                             + "Status Effects:\n";
        foreach (StatusEffect statusEffect in this.hoveredPiece.statuses)
        {
            displayText = displayText + statusEffect.ToString() + "\n";
        }

        pieceIcon.text = "";

        string pieceIconFont = "";
        
        if(this.hoveredPiece.teamColor == TeamColor.Black)
        {
            switch (this.hoveredPiece.pieceType)
            {
                case PieceType.King:
                    pieceIconFont = "k";                
                    break;
                case PieceType.Queen:
                    pieceIconFont = "q";               
                    break;
                case PieceType.Rook:
                    pieceIconFont = "r"; 
                    break;
                case PieceType.Bishop:
                    pieceIconFont = "b"; 
                    break;
                case PieceType.Knight:
                    pieceIconFont = "h"; 
                    break;
                case PieceType.Pawn:
                    pieceIconFont = "p";               
                    break;
            }
        }
        else if(this.hoveredPiece.teamColor == TeamColor.White)
        {
               switch (this.hoveredPiece.pieceType)
            {
                case PieceType.King:
                    pieceIconFont = "l";                
                    break;
                case PieceType.Queen:
                    pieceIconFont = "w";                
                    break;
                case PieceType.Rook:
                    pieceIconFont = "t"; 
                    break;
                case PieceType.Bishop:
                    pieceIconFont = "n"; 
                    break;
                case PieceType.Knight:
                    pieceIconFont = "j"; 
                    break;
                case PieceType.Pawn:
                    pieceIconFont = "o";               
                    break;
            }
        }
        pieceIcon.text += pieceIconFont;

        this.pieceInfoText.SetText(displayText);
    }
}