using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Sprite heartOnSprite;
    public Sprite heartOffSprite;
    Image image;
    int id;

    void Start()
    {
        image = GetComponent<Image>();
        id = transform.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.lives>=id)
            image.sprite = heartOnSprite;
        else
            image.sprite = heartOffSprite;
    }
}
