using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceDialogueBox : MonoBehaviour
{
    Board board;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
    }
}
