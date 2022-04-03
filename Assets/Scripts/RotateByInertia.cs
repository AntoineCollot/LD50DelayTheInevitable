using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByInertia : MonoBehaviour
{
    ComputeInertia intertia;
    float baseRotation;
    public float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        intertia = GetComponentInParent<ComputeInertia>();
        baseRotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, baseRotation + intertia.acceleration.x * multiplier);
    }
}
