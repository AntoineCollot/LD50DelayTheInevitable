using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    int currentId;
    new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(WingsAnimLoop());
    }

    IEnumerator WingsAnimLoop()
    {
        while(true)
        {
            currentId++;
            currentId %= sprites.Length;
            renderer.sprite = sprites[currentId];

            yield return null;
        }
    }
}
