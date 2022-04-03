using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour
{
    public float turnOnDelay = 15;
    public GameObject[] gameObjects;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject go in gameObjects)
            go.SetActive(false);

        Invoke("TurnOn", turnOnDelay);
    }

    void TurnOn()
    {
        GetComponent<Animator>().SetTrigger("TurnOn");
        GetComponent<Collider>().enabled = true;
        GetComponent<LightAttraction>().enabled = true;

        foreach (GameObject go in gameObjects)
            go.SetActive(true);
    }
}
