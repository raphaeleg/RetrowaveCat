using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    private const float MIN_X = -21.7f;
    private const float MAX_X = 49.8f;
    private const float MIN_Y = -0.983f;
    private const float MAX_Y = 2.5f;

    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, MIN_X, MAX_X);
        targetPosition.y = Mathf.Clamp(targetPosition.y, MIN_Y, MAX_Y);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
