using UnityEngine;
using DG.Tweening; // Include DoTween namespace

public class DragPinchZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float minFov = 15f; // Minimum Field of View
    public float maxFov = 90f; // Maximum Field of View
    public float zoomSpeed = 10f; // Speed of zoom
    public float zoomSmoothness = 0.25f; // Smoothing factor for zoom

    [Header("Drag Settings")]
    public Vector2 xAxisLimits = new Vector2(-10f, 10f); // Limits for X-axis movement
    public Vector2 yAxisLimits = new Vector2(-5f, 5f); // Limits for Y-axis movement
    public float dragSpeed = 0.5f; // Speed of drag
    public float dragSmoothness = 0.25f; // Smoothing factor for drag

    private Camera cam;
    private Vector3 dragTargetPosition;
    private float targetFov;

    void Start()
    {
        cam = Camera.main; // Get the main camera
        if (cam == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        // Initialize target values
        dragTargetPosition = cam.transform.position;
        targetFov = cam.fieldOfView;
    }

    void Update()
    {
        HandleZoom();
        HandleDrag();

        // Smoothly move the camera to the target position
        cam.transform.DOMove(dragTargetPosition, dragSmoothness).SetEase(Ease.OutQuad);

        // Smoothly adjust the Field of View
        cam.DOFieldOfView(targetFov, zoomSmoothness).SetEase(Ease.OutQuad);
    }

    private void HandleZoom()
    {
        // For touch input (pinch gesture)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Calculate the difference in distance between the two touches
            float previousDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;

            float distanceDifference = previousDistance - currentDistance;

            AdjustFieldOfView(distanceDifference * zoomSpeed * Time.deltaTime);
        }
        // For mouse scroll (WebGL)
        else if (Input.mouseScrollDelta.y != 0)
        {
            float scrollAmount = Input.mouseScrollDelta.y;
            AdjustFieldOfView(-scrollAmount * zoomSpeed);
        }
    }

    private void AdjustFieldOfView(float delta)
    {
        targetFov = Mathf.Clamp(targetFov + delta, minFov, maxFov);
    }

    private void HandleDrag()
    {
        // For touch drag
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 deltaPosition = touch.deltaPosition;
                UpdateDragTarget(deltaPosition);
            }
        }
        // For mouse drag (WebGL)
        else if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            UpdateDragTarget(mouseDelta * dragSpeed);
        }
    }

    private void UpdateDragTarget(Vector3 delta)
    {
        Vector3 newPosition = dragTargetPosition;

        // Adjust the target position based on delta input
        newPosition.x = Mathf.Clamp(newPosition.x + delta.x * Time.deltaTime, xAxisLimits.x, xAxisLimits.y);
        newPosition.y = Mathf.Clamp(newPosition.y + delta.y * Time.deltaTime, yAxisLimits.x, yAxisLimits.y);

        dragTargetPosition = newPosition;
    }

    // Optional: Methods to adjust limits programmatically
    public void SetZoomLimits(float min, float max)
    {
        minFov = min;
        maxFov = max;
    }

    public void SetDragLimits(Vector2 xLimits, Vector2 yLimits)
    {
        xAxisLimits = xLimits;
        yAxisLimits = yLimits;
    }
}
