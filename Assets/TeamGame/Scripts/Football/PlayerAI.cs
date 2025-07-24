using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAI : MonoBehaviour
{
    public enum Team { Ally, Enemy }
    public enum Role { Defender, Midfielder, Striker }

    public Team team;
    public Role role;

    public float moveSpeed = 5f; // Увеличено для большей динамики
    public Transform ball;
    public Transform goalToAttack;

    private Rigidbody2D rb;
    private float passCooldown = 0.8f; // Уменьшено для более частых пасов
    private float passTimer = 0f;
    private float avoidRadius = 1.5f; // Увеличено для меньшего столкновения
    private static Transform currentChaser; // Один игрок за мячом в команде
    private Vector2 fieldCenter = Vector2.zero;
    private Vector2 lastVelocity; // Для сглаживания движения

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        lastVelocity = Vector2.zero;
    }

    private void Update()
    {
        passTimer -= Time.deltaTime;

        Transform ballOwner = ball.GetComponent<BallController>().currentOwner;
        bool isAllyOwner = ballOwner != null && ballOwner.CompareTag(gameObject.tag);
        float distToBall = Vector2.Distance(transform.position, ball.position);

        // Переопределяем охотника, если мяч далеко или нет охотника
        if (currentChaser == null || Vector2.Distance(currentChaser.position, ball.position) > 5f)
        {
            currentChaser = FindClosestPlayerToBall();
        }

        Vector2 target;
        if (transform == currentChaser && !isAllyOwner)
        {
            // Преследуем мяч
            target = (Vector2)ball.position;
        }
        else if (isAllyOwner && ballOwner != transform)
        {
            // Позиционируемся для паса
            Vector2 offset = ((Vector2)goalToAttack.position - (Vector2)transform.position).normalized * 3f;
            target = (Vector2)ballOwner.position + offset;
            target = ClampToField(target);
        }
        else
        {
            // Динамическая тактическая позиция
            target = GetTacticalPosition(distToBall);
        }

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 avoidance = ComputeAvoidance();
        Vector2 finalDirection = (direction + avoidance).normalized;

        // Сглаживание движения для устранения тряски
        Vector2 targetVelocity = finalDirection * moveSpeed;
        rb.velocity = Vector2.Lerp(lastVelocity, targetVelocity, Time.deltaTime * 10f);
        lastVelocity = rb.velocity;

        // Взаимодействие с мячом
        if (distToBall < 0.7f && passTimer <= 0f && (!isAllyOwner || ballOwner == transform))
        {
            BallController ballCtrl = ball.GetComponent<BallController>();
            ballCtrl.SetOwner(transform);
            TryPassOrShoot();
            passTimer = passCooldown;
        }
    }

    Transform FindClosestPlayerToBall()
    {
        PlayerAI[] allPlayers = FindObjectsOfType<PlayerAI>();
        return allPlayers
            .Where(p => p.team == team)
            .OrderBy(p => Vector2.Distance(p.transform.position, ball.position))
            .FirstOrDefault()?.transform;
    }

    void TryPassOrShoot()
    {
        BallController ballCtrl = ball.GetComponent<BallController>();
        float distToGoal = Vector2.Distance(transform.position, goalToAttack.position);

        if (distToGoal < 4.5f)
        {
            // Удар по воротам
            ballCtrl.ShootTowards((Vector2)goalToAttack.position + Random.insideUnitCircle * 0.6f);
        }
        else
        {
            // Пас ближайшему открытому игроку
            GameObject[] teammates = GameObject.FindGameObjectsWithTag(gameObject.tag);
            Transform bestTarget = null;
            float minDistance = float.MaxValue;

            foreach (var mate in teammates)
            {
                if (mate.transform == transform) continue;
                float dist = Vector2.Distance(transform.position, mate.transform.position);
                float distToBall = Vector2.Distance(mate.transform.position, ball.position);
                if (dist < 7f && distToBall > 1.5f && dist < minDistance)
                {
                    minDistance = dist;
                    bestTarget = mate.transform;
                }
            }

            if (bestTarget != null)
            {
                ballCtrl.PassTo((Vector2)bestTarget.position + Random.insideUnitCircle * 0.5f);
            }
            else
            {
                // Пинаем мяч вперед, если нет цели
                ballCtrl.PassTo((Vector2)transform.position + ((Vector2)goalToAttack.position - (Vector2)transform.position).normalized * 3f);
            }
        }
    }

    // Перегрузка для сброса игроков (без параметра)
    public Vector2 GetTacticalPosition()
    {
        return GetTacticalPosition(0f); // Используем 0 как значение по умолчанию
    }

    public Vector2 GetTacticalPosition(float distToBall)
    {
        float x = 0f, y = 0f;
        int direction = team == Team.Ally ? -1 : 1;

        // Динамическая позиция в зависимости от расстояния до мяча
        float spread = distToBall > 3f ? 2f : 1f; // Больше разброс, если мяч далеко
        switch (role)
        {
            case Role.Defender:
                x = Random.Range(-spread, spread) * direction;
                y = team == Team.Ally ? -3.5f : 3.5f;
                break;
            case Role.Midfielder:
                x = Random.Range(-2.5f, 2.5f);
                y = Random.Range(-2.5f, 2.5f);
                break;
            case Role.Striker:
                x = Random.Range(-spread, spread) * direction;
                y = team == Team.Ally ? 3.5f : -3.5f;
                break;
        }

        return ClampToField(fieldCenter + new Vector2(x, y));
    }

    Vector2 ClampToField(Vector2 pos)
    {
        return new Vector2(
            Mathf.Clamp(pos.x, -2.8f, 2.8f),
            Mathf.Clamp(pos.y, -4.8f, 4.8f)
        );
    }

    Vector2 ComputeAvoidance()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidRadius);
        Vector2 avoidDir = Vector2.zero;

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && (hit.CompareTag("Ally") || hit.CompareTag("Enemy")))
            {
                Vector2 dir = (Vector2)transform.position - (Vector2)hit.transform.position;
                float dist = dir.magnitude;
                if (dist > 0)
                    avoidDir += dir.normalized / Mathf.Max(dist, 0.1f);
            }
        }

        return avoidDir * 1.2f; // Усилено избегание
    }
}