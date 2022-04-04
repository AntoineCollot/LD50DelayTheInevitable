using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void PlayIntro()
    {
        Invoke("PlayIntroDelayed", 1.5f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CancelInvoke();
            director.Stop();
            OnIntroFinished();
        }
    }

    void PlayIntroDelayed()
    {
        director.Play();
        Invoke("OnIntroFinished", (float)director.playableAsset.duration);
    }

    void OnIntroFinished()
    {
        GameManager.Instance.StartGame();
    }
}
