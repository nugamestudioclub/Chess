using UnityEngine;

public class RotateSphereSkybox : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;

    void Update()
    {
        transform.Rotate(0f, Time.deltaTime * rotationSpeed, 0f);
    }
}