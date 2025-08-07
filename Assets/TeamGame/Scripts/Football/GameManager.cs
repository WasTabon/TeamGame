using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public float matchDuration = 10f;
    private float timer;
    private int allyScore = 0;
    private int enemyScore = 0;

    [SerializeField] private List<GameObject> _players;
    [SerializeField] private TextMeshProUGUI _matchScoreText; // 👈 Добавлено

    private List<Vector3> _playerPositions;
    public bool _startGame;

    private Coroutine _scoreCoroutine;
    
    private List<string> _cachedMatchResult;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerPositions = new List<Vector3>();
        for (int i = 0; i < _players.Count; i++)
        {
            _playerPositions.Add(_players[i].transform.localPosition);
        }

        timer = matchDuration;
        _matchScoreText.text = "0 - 0"; // начальное значение
    }

    private void Update()
    {
        if (_startGame)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                _startGame = false;
                StopCoroutine(_scoreCoroutine);
                EnsureEnemyWins();

                UIController.Instance.FinishMatch(_cachedMatchResult);
                Debug.Log($"Match Over! Ally: {allyScore} - Enemy: {enemyScore}");
            }
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].transform.localPosition = _playerPositions[i];
        }

        // Генерируем результат заранее
        _cachedMatchResult = MatchResultSystem.Instance.GenerateRandomMatchResult();

        timer = matchDuration;
        allyScore = 0;
        enemyScore = 0;
        _matchScoreText.text = "0 - 0";
        _startGame = true;
        Time.timeScale = 1;

        if (_scoreCoroutine != null)
            StopCoroutine(_scoreCoroutine);

        _scoreCoroutine = StartCoroutine(RandomScoreRoutine());

        Debug.Log("Game Started!");
    }

    private IEnumerator RandomScoreRoutine()
    {
        while (_startGame)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));

            // Случайно прибавляем очко Ally или Enemy (чаще врагу)
            if (Random.value < 0.4f)
                allyScore++;
            else
                enemyScore++;

            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        _matchScoreText.text = $"{allyScore} - {enemyScore}";
    }

    private void EnsureEnemyWins()
    {
        if (enemyScore <= allyScore)
        {
            enemyScore = allyScore + 1;
            UpdateScoreText();
        }
    }

    public void ScoreGoal(GameObject goal)
    {
        if (goal.CompareTag("AllyGoal"))
            enemyScore++;
        else if (goal.CompareTag("EnemyGoal"))
            allyScore++;

        UpdateScoreText();
        Debug.Log($"Score! Ally: {allyScore} - Enemy: {enemyScore}");
        ResetBallAndPlayers();
    }

    private void ResetBallAndPlayers()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            ball.transform.position = Vector2.zero;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ball.GetComponent<BallController>().SetOwner(null);
        }

        PlayerAI[] players = FindObjectsOfType<PlayerAI>();
        foreach (var player in players)
        {
            player.transform.position = player.GetTacticalPosition();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
