using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteHuman : MonoBehaviour
{
    Animator anim;
    FollowCursor controls;
    public float animTime = 2;
    bool isBiting = false;

    [Header("Target")]
    public Transform biteTarget;
    public float biteArea;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controls = GetComponent<FollowCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
            return;

        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !isBiting)
        {
            StartCoroutine(BiteAnim());
        }
    }

    IEnumerator BiteAnim()
    {
        anim.SetTrigger("Bite");
        controls.enabled = false;
        isBiting = true;

        yield return new WaitForSeconds(animTime * 0.5f);

        //Check if successfull
        GameManager.Instance.AddScore(1);
        yield return new WaitForSeconds(animTime * 0.5f);

        isBiting = false;
        controls.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (biteTarget != null)
            Gizmos.DrawWireSphere(biteTarget.position, biteArea);
    }
}
