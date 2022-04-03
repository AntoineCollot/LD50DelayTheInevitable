using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    TextMeshProUGUI text;
    public MaterialInstance screenMat;
    float baseIntensity;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        baseIntensity = screenMat.InstancedMat.GetFloat("_Intensity");
        GameManager.Instance.onScoreMajorIncrease.AddListener(OnScoreIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.score.ToString("N0");
    }

    [ContextMenu("FlashScreen")]
    public void OnScoreIncrease()
    {
        StartCoroutine(LitScreenAnim());
    }

    IEnumerator LitScreenAnim()
    {
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / 0.5f;
            screenMat.InstancedMat.SetFloat("_Intensity", baseIntensity + Mathf.PingPong(t*8, 4f));

            yield return null;
        }
    }
}
