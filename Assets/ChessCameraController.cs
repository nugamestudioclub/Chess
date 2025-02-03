using UnityEngine;

public class ChessCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 15f;

    private float currentZoom;
    private float yaw;
    private float pitch;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private bool reset = true;

    private bool atStartingPoint = true;
    
    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("ChessCameraController: Target is not assigned!");
            enabled = false;
            return;
        }

        startingPosition = transform.position;
        startingRotation = transform.rotation;
        currentZoom = (startingPosition - target.position).magnitude;
        ResetCamera();
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();

        if (!atStartingPoint && Input.GetKeyDown(KeyCode.Space))
        {
            atStartingPoint = true;
            ResetCamera();
        }
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            atStartingPoint = false;
            reset = false;
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -30f, 60f);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    private void UpdateCameraPosition()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back * currentZoom;

        transform.position = target.position + direction;
        transform.LookAt(target);
    }

    private void ResetCamera()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        yaw = startingRotation.eulerAngles.y;
        pitch = startingRotation.eulerAngles.x;
        currentZoom = (startingPosition - target.position).magnitude;
        reset = false;
    }
}
