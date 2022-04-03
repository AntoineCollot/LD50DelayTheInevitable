using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLightWind : MonoBehaviour
{
    public float amplitude = 2;
    public float amplitudeDetail = 2;
    public float period = 1;
    public float periodDetail = 1;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * period) *amplitude + Mathf.PerlinNoise(32.4f, Time.timeSinceLevelLoad* periodDetail) * -amplitudeDetail);
    }
}
