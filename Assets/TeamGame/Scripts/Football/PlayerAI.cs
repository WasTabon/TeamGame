using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAI : MonoBehaviour
{
    public enum Team { Ally, Enemy }
    public enum Role { Defender, Midfielder, Striker }

    public Team team;
    public Role role;

    public float moveSpeed = 2f;
    public Transform ball;
    public Transform goalToAttack;
    public Vector2 homePosition; // Позиция, где игрок "пасётся"

    private Rigidbody2D rb;
    private float passCooldown = 2f;
    private float passTimer = 0f;

    private float engageRadius = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        passTimer -= Time.deltaTime;

        Vector2 target;

        float distToBall = Vector2.Distance(transform.position, ball.position);

        if (distToBall < engageRadius)
        {
            // Преследовать мяч
            target = ball.position;
        }
        else
        {
            // Возврат в зону
            target = homePosition;
        }

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Пытаемся взять мяч
        if (distToBall < 0.5f && passTimer <= 0f)
        {
            BallController ballCtrl = ball.GetComponent<BallController>();
            if (ballCtrl.currentOwner == null || ballCtrl.currentOwner == transform)
            {
                ballCtrl.SetOwner(transform);
                TryPassOrShoot();
                passTimer = passCooldown;
            }
        }
    }

    void TryPassOrShoot()
    {
        BallController ballCtrl = ball.GetComponent<BallController>();
        if (Vector2.Distance(transform.position, goalToAttack.position) < 3f)
        {
            ballCtrl.ShootTowards(goalToAttack.position);
        }
        else
        {
            GameObject[] teammates = GameObject.FindGameObjectsWithTag(team == Team.Ally ? "Ally" : "Enemy");
            Transform closest = null;
            float minDist = Mathf.Infinity;

            foreach (var mate in teammates)
            {
                if (mate.transform == transform) continue;
                float dist = Vector2.Distance(transform.position, mate.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = mate.transform;
                }
            }

            if (closest != null)
                ballCtrl.PassTo(closest.position);
        }
    }
}
