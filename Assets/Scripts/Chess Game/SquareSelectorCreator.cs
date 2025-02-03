using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    private int counter = 0;

    public void CreateSquareSelectors(Board board)
    {
        StartCoroutine(CreateSquareDelay(board));

    }

    private IEnumerator CreateSquareDelay(Board board)
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                counter++;
                CreateSquareSelector(board, new Vector2Int(x, y));
                yield return new WaitForSeconds(0.05f);
            }
        }

        ChessPlayer.instance.canClick = true;
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

        bool xEven = position.x % 2 == 0;
        bool yEven = position.y % 2 == 0;

        if (xEven == yEven)
        {
            square.GetComponent<Renderer>().material = whiteMaterial;
        }
        else
        {
            square.GetComponent<Renderer>().material = blackMaterial;
        }

        square.position = position;

        selector.layer = 3; // square layer

        selector.name = "square" + position.x + "_" + position.y;
    }
}
