using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesAnimation : MonoBehaviour
{
    float speed;
    float baseRotation;
    public float amplitude = 5;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1f, 1.3f);
        baseRotation = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, baseRotation + Mathf.Sin(Time.time * speed)* amplitude));
    }
}
