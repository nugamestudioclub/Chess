using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelector : MonoBehaviour
{
    public Vector2Int position;

    public GameObject moveTileIndicator;
    public GameObject capturableTileIndicator;

    public void ToggleMoveTileIndicator(bool enabled)
    {
        moveTileIndicator.SetActive(enabled);
    }
    
    
    public void ToggleCapturableTileIndicator(bool enabled)
    {
        capturableTileIndicator.SetActive(enabled);
    }


    
}
