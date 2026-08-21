using UnityEngine;

public class GridSnapper : MonoBehaviour
{
    [Header("Grid Settings")]
    [Tooltip("The size of each grid cell.")]
    public float gridSize = 1.0f;

    [Tooltip("Offset height (Y-axis) to keep the object above the ground.")]
    public float yOffset = 0.5f;

    [Header("Raycast Settings")]
    [Tooltip("Layer mask to restrict what surfaces the mouse can raycast against.")]
    public LayerMask groundLayer;

    private Camera mainCamera;

    public GameObject Piezainstancias;

    void Start()
    {
        // Cache the main camera for better performance
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdatePositionWithMouse();
    }

    private void UpdatePositionWithMouse()
    {
        // 1. Create a ray from the camera through the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // 2. Cast the ray to find where it hits the ground plane
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 hitPoint = hit.point;

            // 3. Snap the X and Z coordinates to the grid system
            float snappedX = Mathf.Round(hitPoint.x / gridSize) * gridSize;
            float snappedZ = Mathf.Round(hitPoint.z / gridSize) * gridSize;

            // 4. Apply the snapped coordinates to the object
            transform.position = new Vector3(snappedX, yOffset, snappedZ);
        }
    }

    private void OnMouseDown()
    {
        GameObject clon = Instantiate(Piezainstancias, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
