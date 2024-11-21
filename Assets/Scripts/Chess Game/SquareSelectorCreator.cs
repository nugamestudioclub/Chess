using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;

    public void CreateSquareSelectors(Board board)
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                // wait a quarter of a second
                StartCoroutine(CreateSquareDelay(board, x, y));
            }
        }
    }

    private IEnumerator CreateSquareDelay(Board board, int x, int y)
    {
        yield return new WaitForSeconds(1);
        CreateSquareSelector(board, new Vector2Int(x, y));
    }

    public void CreateSquareSelector(Board board, Vector2Int position)
    {
        GameObject selector = Instantiate(squarePrefab);
        selector.transform.parent = board.gameObject.transform;

        selector.transform.position = board.PosToVect(position);

        var collider = selector.AddComponent<BoxCollider>();

        Vector3 size = board.PosToVect(position + Vector2Int.up + Vector2Int.right) - selector.transform.position;

        //collider.size = new Vector3(size.x, size.y, size.z);

        selector.transform.localScale = new Vector3(size.x, .05f, size.z);

        var square = selector.AddComponent<SquareSelector>();

        square.position = position;

        selector.layer = 3; // square layer

        selector.name = "square" + position.x + "_" + position.y;
    }
}
