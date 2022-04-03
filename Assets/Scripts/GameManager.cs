using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static float score;
    public static bool GameIsOver = false;
    public static bool GameHasStarted = false;
    public static bool GameIsPlaying { get => !GameIsOver && GameHasStarted; }
    public static float timeSinceGameStarted;
    public static int lives;

    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    public static GameManager Instance;
    public UnityEvent onScoreMajorIncrease = new UnityEvent();


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        score = 0;
        timeSinceGameStarted = 0;
        lives = 2;
        GameIsOver = false;
        GameHasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsPlaying)
        {
            score += Time.deltaTime * 0.2f;
            timeSinceGameStarted += Time.deltaTime;
        }
    }

    public void AddScore(float value)
    {
        score += value;
        onScoreMajorIncrease.Invoke();
    }

    public void StartGame()
    {
        timeSinceGameStarted = 0;
        GameHasStarted = true;
        GameIsOver = false;
        score = 0;
        lives = 2;
        onGameStart.Invoke();
    }

    public void GameOver()
    {
        GameIsOver = true;
        onGameOver.Invoke();
    }

    public void OnMoskitoKill()
    {
        lives--;
        if (lives < 0 && !GameIsOver)
            GameOver();
    }
}
