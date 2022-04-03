using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour
{
    public float turnOnDelay = 15;
    public GameObject[] gameObjects;
    public bool playSound = true;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject go in gameObjects)
            go.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
    }

    void OnGameStart()
    {
        Invoke("TurnOn", turnOnDelay);
    }

    void TurnOn()
    {
        GetComponent<Animator>().SetTrigger("TurnOn");
        GetComponent<Collider>().enabled = true;
        GetComponent<LightAttraction>().enabled = true;
        if(playSound)
            SoundManager.PlaySound(2);

        foreach (GameObject go in gameObjects)
            go.SetActive(true);
    }
}
