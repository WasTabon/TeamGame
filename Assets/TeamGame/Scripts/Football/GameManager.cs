using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float matchDuration = 15f;
    private float timer;

    void Start()
    {
        timer = matchDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Debug.Log("Match Over");
            Time.timeScale = 0;
        }
    }
}