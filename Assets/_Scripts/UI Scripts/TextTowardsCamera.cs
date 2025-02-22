using UnityEngine;

public class TextTowardsCamera : MonoBehaviour
{
    public Camera cam; // Reference to the camera (you can assign it in the Inspector)
    
    void Start()
    {
        if (cam == null)
            cam = Camera.main; // Automatically use the main camera if none is assigned
    }

    void Update()
    {
        Vector3 targetPosition = cam.transform.position;
        targetPosition.y = transform.position.y; // Keep the y-axis the same (prevent tilting up/down)
        
        // Make the object face the camera
        transform.LookAt(targetPosition);

        // Flip the object to make it readable (adjust the local rotation)
        transform.Rotate(0f, 180f, 0f); // Adjust this value if needed to flip it correctly
    }
}