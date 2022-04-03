using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    bool isLoading = false;
    public int sceneId = 0;

    public void Load()
    {
        if (isLoading)
            return;

        isLoading = true;
        SceneManager.LoadScene(sceneId);
    }
}
