using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    Camera cam;

    [Range(0,1)] public float smooth;
    Vector3 refPosition;
    Vector3 lastPosition;

    new AudioSource audio;

    [Header("Direction")]
    Direction dir;
    float lastTurnTime;
    const float TURN_INTERVAL = .3f;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
            return;

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Mathf.Abs(cam.transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, cam.ScreenToWorldPoint(screenPosition), ref refPosition, smooth);


        Turn();

        audio.volume = Vector2.Distance(lastPosition, transform.position) / Time.deltaTime * 0.03f;
        lastPosition = transform.position;
    }

    void Turn()
    {
        float movement = transform.position.x - lastPosition.x;
        if (Time.time < lastTurnTime + TURN_INTERVAL)
            return;
        if (movement > 0 && dir != Direction.Right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            dir = Direction.Right;
            lastTurnTime = Time.time;
        }
        else if (movement < 0 && dir != Direction.Left)
        {
            transform.localScale = Vector3.one;
            dir = Direction.Left;
            lastTurnTime = Time.time;
        }
    }
}
