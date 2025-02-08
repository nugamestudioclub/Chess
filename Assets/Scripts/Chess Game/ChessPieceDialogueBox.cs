using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChessPieceDialogueBox : MonoBehaviour
{
    Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = Board.instance;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
        if (this.board.showPieceInfo)
        {
            this.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
        else
        {
            this.GetComponent<CanvasGroup>().alpha = 0.0f;
        }
        //this.GetComponentInChildren<TMP_Text>().SetText("Test");
    }
}