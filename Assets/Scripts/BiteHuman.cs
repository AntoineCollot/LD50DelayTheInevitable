using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiteHuman : MonoBehaviour
{
    Animator anim;
    FollowCursor controls;
    public float bitePreparationTime = 0.3f;
    public float biteCooldownTime = 1;
    public bool isBiting { get; private set; }

    public UnityEvent onSuccessfullBite = new UnityEvent();

    [Header("Target")]
    public Transform biteTarget;
    public Transform biteOrigin;
    public float biteArea;
    public Animator humanAnim;

    public static BiteHuman Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isBiting = false;
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
        SoundManager.PlaySound(1);

        yield return new WaitForSeconds(bitePreparationTime);

        Vector2 toTarget = biteTarget.position - biteOrigin.position;
        //Check if successfull
        if(toTarget.magnitude < biteArea)
        {
            GameManager.Instance.AddScore(1);
            humanAnim.SetTrigger("Bite");
            onSuccessfullBite.Invoke();
        }

        yield return new WaitForSeconds(biteCooldownTime);

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
