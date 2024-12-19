using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] public float speed = -16.0f;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            speed,
            rb.linearVelocity.y
        );
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy")) { return; }
        else if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.GetType().ToString() == "UnityEngine.BoxCollider2D")
            {
                EventManager.TriggerEvent("CaughtMouse", 1);
                Destroy(gameObject);
                return;
            }
        }
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        speed *= -1f;
    }
}
