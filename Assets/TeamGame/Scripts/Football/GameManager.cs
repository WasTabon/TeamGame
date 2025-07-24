using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for easy access
    public float matchDuration = 90f; // Longer match for testing
    private float timer;
    private int allyScore = 0;
    private int enemyScore = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = matchDuration;
        ResetBallAndPlayers();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Debug.Log($"Match Over! Ally: {allyScore} - Enemy: {enemyScore}");
            Time.timeScale = 0;
        }
    }

    public void ScoreGoal(GameObject goal)
    {
        if (goal.CompareTag("AllyGoal"))
            enemyScore++;
        else if (goal.CompareTag("EnemyGoal"))
            allyScore++;

        Debug.Log($"Score! Ally: {allyScore} - Enemy: {enemyScore}");
        ResetBallAndPlayers();
    }

    void ResetBallAndPlayers()
    {
        // Reset ball
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            ball.transform.position = Vector2.zero;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ball.GetComponent<BallController>().SetOwner(null);
        }

        // Reset players to tactical positions
        PlayerAI[] players = FindObjectsOfType<PlayerAI>();
        foreach (var player in players)
        {
            player.transform.position = player.GetTacticalPosition(); // Теперь работает
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}