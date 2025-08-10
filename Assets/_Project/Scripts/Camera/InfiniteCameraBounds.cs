using UnityEngine;

namespace SpaceShooter.CameraSystem
{
    public class InfiniteCameraBounds : MonoBehaviour
    {
        [SerializeField] private float _indent = 0.1f;

        private Camera _mainCamera;
        
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPosition.x > 1 + _indent)
            {
                transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(0, viewportPosition.y, viewportPosition.z));
            }
            else if (viewportPosition.x < 0 - _indent)
            {
                transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(1, viewportPosition.y, viewportPosition.z));
            }

            if (viewportPosition.y > 1 + _indent)
            {
                transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 0, viewportPosition.z));
            }
            else if (viewportPosition.y < 0 - _indent)
            {
                transform.position = _mainCamera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 1, viewportPosition.z));
            }
        }
    }

}
