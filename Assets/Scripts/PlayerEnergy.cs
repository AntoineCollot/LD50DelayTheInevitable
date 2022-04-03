using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public float energyDepleteTime = 10;
    public float energy01 { get; private set; }

    public static PlayerEnergy Instance;

    public bool HasEnergy { get =>energy01>0.01f; }

    [Header("Visuals")]
    public MaterialInstance mat;
    public Transform glowCircle;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GetComponent<BiteHuman>().onSuccessfullBite.AddListener(OnBite);
        energy01 = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
            return;

        energy01 -= Time.deltaTime / energyDepleteTime;
        energy01 = Mathf.Clamp01(energy01);

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        float flash = Mathf.Sin(Time.timeSinceLevelLoad * Mathf.Lerp(10, 2, energy01));
        if (flash > Mathf.Lerp(0.5f, 0.1f, energy01))
            flash = 1;
        else
            flash = 0;

        if (energy01 == 0)
            flash = 0;
        else if (energy01 > 0.3f)
            flash = 1;

        mat.InstancedMat.SetFloat("_On", flash);

        glowCircle.localScale = Vector3.one * Mathf.Lerp(3, 10, energy01);
    }

    void OnBite()
    {
        energy01 = 1;
    }
}
