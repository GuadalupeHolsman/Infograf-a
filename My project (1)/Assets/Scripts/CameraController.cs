using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;            // A qué objeto mira la cámara
    public float distance = 10.0f;      // Distancia desde el target
    public float zoomSpeed = 2.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 30.0f;

    public float rotationSpeed = 100.0f;

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, 0, -distance);
        UpdateCameraPosition();
    }

    void Update()
    {
        // ----------- MOUSE ROTATION ------------
        if (Input.GetMouseButton(1)) // botón derecho del mouse
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, -85, 85); // evita que la cámara dé la vuelta
        }

        // ----------- TOUCH ROTATION (1 dedo) ------------
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * rotationSpeed * 0.01f * Time.deltaTime;
                currentY -= touch.deltaPosition.y * rotationSpeed * 0.01f * Time.deltaTime;
                currentY = Mathf.Clamp(currentY, -85, 85);
            }
        }

        // ----------- ZOOM (scroll o pinch) ------------
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Zoom táctil (pinch con dos dedos)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float prevMag = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currMag = (touch0.position - touch1.position).magnitude;

            float deltaMag = prevMag - currMag;
            scroll = deltaMag * 0.01f; // convierte el "pinch" a un valor de scroll
        }

        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        offset = new Vector3(0, 0, -distance);
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target);
    }
}