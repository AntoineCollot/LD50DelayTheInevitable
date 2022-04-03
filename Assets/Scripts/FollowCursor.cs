using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    Camera cam;

    [Range(0,1)] public float smooth;
    Vector3 refPosition;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
            return;

        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Mathf.Abs(cam.transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, cam.ScreenToWorldPoint(screenPosition), ref refPosition, smooth);
    }
}
