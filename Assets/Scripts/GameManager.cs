using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static float score;
    public static bool GameIsOver = false;

    public static GameManager Instance;
    public UnityEvent onScoreMajorIncrease = new UnityEvent();


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameIsOver)
            score += Time.deltaTime * 0.2f;
    }

    public void AddScore(float value)
    {
        score += value;
        onScoreMajorIncrease.Invoke();
    }
}
