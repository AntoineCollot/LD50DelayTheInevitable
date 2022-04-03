using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMoskitoLum : MonoBehaviour
{
   [Range(0,1)] public float intensity = 0;
    public Transform halo;
    MaterialInstance mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MaterialInstance>();
    }

    // Update is called once per frame
    void Update()
    {
        mat.InstancedMat.SetFloat("_On", intensity);
        halo.localScale = Vector3.one * 10 * intensity;
    }
}
