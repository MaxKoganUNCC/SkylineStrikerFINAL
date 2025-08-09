using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform cameraTransform;

    private Vector3 cameraOffset;
    private float cameraMoveSpeed;

    private void Start ()
    {
        cameraOffset = new Vector3 (0f, 6f, -10f);
        cameraMoveSpeed = 5f;
    }

    private void Update ()
    {
        // Find position
        Vector3 cameraEndPosition = playerManager.playerTransform.position + cameraOffset;

        // Move to poisition
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraEndPosition, Time.deltaTime * cameraMoveSpeed);

    }
}
