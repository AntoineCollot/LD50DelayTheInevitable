using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttraction : MonoBehaviour
{
    [Min(0)] public float range = 5;
    public Vector3 offset;

    public static List<LightAttraction> lights = new List<LightAttraction>();

    public Vector3 AttractionPosition { get => transform.TransformPoint(offset); }

    void OnEnable()
    {
        lights.Add(this);
    }

    private void OnDisable()
    {
        lights.Remove(this);
    }

    public static LightAttraction GetMostAttractiveLight(Vector3 pos)
    {
        float minDist = Mathf.Infinity;
        LightAttraction closestLight = null;
        foreach (LightAttraction light in lights)
        {
            float dist = Vector3.Distance(pos, light.AttractionPosition) / light.range;
            if (dist < minDist)
            {
                minDist = dist;
                closestLight = light;
            }
        }

        return closestLight;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(AttractionPosition, range);
    }
}
