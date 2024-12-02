using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceDialogueBox : MonoBehaviour
{
    Board board;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
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
    }
}
