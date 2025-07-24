using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    public Transform currentOwner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        currentOwner = null; // Ensure null at start
    }

    void FixedUpdate()
    {
        if (currentOwner != null)
        {
            rb.MovePosition(currentOwner.position + Vector3.up * 0.3f); // Adjusted for ball size (0.5)
        }
        else
        {
            // Slow down ball when not owned
            rb.velocity *= 0.95f;
        }
    }

    public void SetOwner(Transform newOwner)
    {
        currentOwner = newOwner;
        rb.velocity = Vector2.zero; // Stop ball when picked up
    }

    public void PassTo(Vector2 target)
    {
        currentOwner = null;
        Vector2 direction = (target - rb.position).normalized;
        rb.velocity = direction * 6f; // Slightly faster pass
    }

    public void ShootTowards(Vector2 target)
    {
        currentOwner = null;
        Vector2 direction = (target - rb.position).normalized;
        rb.velocity = direction * 10f; // Faster shot
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            GameManager.Instance.ScoreGoal(other.gameObject);
            transform.position = Vector2.zero; // Reset ball
            rb.velocity = Vector2.zero;
            currentOwner = null;
        }
    }
}