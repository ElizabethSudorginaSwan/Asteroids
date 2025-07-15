using UnityEngine;

public class InfiniteCameraBounds : MonoBehaviour
{
    private Camera mainCamera;
    private float indent = 0.1f;
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        
        if (viewportPosition.x > 1 + indent)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0, viewportPosition.y, viewportPosition.z));
        }
        else if (viewportPosition.x < 0 - indent)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(1, viewportPosition.y, viewportPosition.z));
        }

        if (viewportPosition.y > 1 + indent)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 0, viewportPosition.z));
        }
        else if (viewportPosition.y < 0 - indent)
        {
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 1, viewportPosition.z));
        }
    }
}
