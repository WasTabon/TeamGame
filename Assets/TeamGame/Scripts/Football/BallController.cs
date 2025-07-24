using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    public Transform currentOwner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (currentOwner != null)
        {
            rb.MovePosition(currentOwner.position + Vector3.up * 0.2f); // «прилипание»
        }
    }

    public void SetOwner(Transform newOwner)
    {
        currentOwner = newOwner;
    }

    public void PassTo(Vector2 target)
    {
        currentOwner = null;
        Vector2 direction = (target - rb.position).normalized;
        rb.velocity = direction * 5f;
    }

    public void ShootTowards(Vector2 target)
    {
        currentOwner = null;
        Vector2 direction = (target - rb.position).normalized;
        rb.velocity = direction * 8f;
    }
}