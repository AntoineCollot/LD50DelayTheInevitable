using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonTurnOnLight : MonoBehaviour
{
    Button button;
    public GameObject LightOn;
    public GameObject LightOff;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
       // LightOn.SetActive()
    }
}
