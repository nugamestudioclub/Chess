using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    public void CreateSquareSelectors(Board board)
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                CreateSquareSelector(board, new Vector2Int(x, y));
            }
        }
    }

    public void CreateSquareSelector(Board board, Vector2Int position)
    {
        GameObject selector = new GameObject();
        selector.transform.parent = board.gameObject.transform;

        selector.transform.position = board.PosToVect(position);

        var collider = selector.AddComponent<BoxCollider>();

        Vector3 size = board.PosToVect(position + Vector2Int.up + Vector2Int.right) - selector.transform.position; 

        collider.size = new Vector3(size.x, size.y, size.z);

        var square = selector.AddComponent<SquareSelector>();

        square.position = position;

        selector.layer = 3; // square layer

        selector.name = "square" + position.x + "_" + position.y;
    }
}
