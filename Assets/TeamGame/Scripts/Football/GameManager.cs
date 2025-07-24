using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for easy access
    public float matchDuration = 90f; // Longer match for testing
    private float timer;
    private int allyScore = 0;
    private int enemyScore = 0;

    [SerializeField] private List<GameObject> _players;

    private List<Vector3> _playerPositions;

    public bool _startGame;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerPositions = new List<Vector3>();

        for (int i = 0; i < _players.Count; i++)
        {
            _playerPositions.Add(new Vector3());
            _playerPositions[i] = _players[i].transform.localPosition;
        }
        
        timer = matchDuration;
        //Time.timeScale = 0;
    }

    private void Update()
    {
        if (_startGame)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                _startGame = false;
                Debug.Log($"Match Over! Ally: {allyScore} - Enemy: {enemyScore}");
                //Time.timeScale = 0;
            }
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].transform.localPosition = _playerPositions[i];
        }
        timer = matchDuration;
        //ResetBallAndPlayers();
        Time.timeScale = 1;
        Debug.Log("Game Started!");
        _startGame = true;
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

    private void ResetBallAndPlayers()
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
            player.transform.position = player.GetTacticalPosition();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}