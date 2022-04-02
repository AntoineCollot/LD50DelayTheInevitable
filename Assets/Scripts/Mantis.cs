using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis : MonoBehaviour
{
    [Header("Movement")]
    public float moveToTargetSpeed = 1;
    float movement;
    float refMovement;
    public float movementSmooth = 0.1f;
    Vector2 spawnPosition;

    [Header("Attack")]
    public float triggerAttackDistance = 2;
    InsectBoid target;
    public enum State { MovingToTarget, Spooked, Attacking }
    State state;

    [Header("Player")]
    public float flightZoneRadius;
    public float spookedMoveSpeed =2;
    float spookStateDuration = 5;
    float spookTime;

    [Header("Animations")]
    Animator anim;

    Transform player;

    public bool IsSpookedByPlayer { get => Vector2.Distance(transform.position, player.position) < flightZoneRadius; }

    // Start is called before the first frame update
    void Start()
    {
        target = SelectTarget();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toTarget = target.transform.position - transform.position;
        Vector3 playerToMantis = transform.position - player.position;
        float direction = Mathf.Sign(toTarget.x);

        float targetMovement = 0;

        //Remove spooked state
        if (Time.time > spookTime + spookStateDuration)
            state = State.MovingToTarget;

        //Find the state
        if (IsSpookedByPlayer)
            Spook();
        //Look for attack
        if (state!=State.Spooked && Mathf.Abs(toTarget.x) < triggerAttackDistance)
            Attack();

        switch (state)
        {
            default:
            case State.MovingToTarget:
                targetMovement = direction * moveToTargetSpeed;
                break;

            case State.Spooked:
                float toSpawnPos = spawnPosition.x - transform.position.x;
                targetMovement = Mathf.Sign(playerToMantis.x) * Mathf.Clamp(InsectBoid.Inv(playerToMantis.magnitude, 2), 0,10) + Mathf.Sign(toSpawnPos) * spookedMoveSpeed;
                break;

            case State.Attacking:
                break;
        }

        movement = Mathf.SmoothDamp(movement, targetMovement, ref refMovement, movementSmooth);
        transform.Translate(Vector3.right * movement * Time.deltaTime);
        anim.SetFloat("Movement", movement);

    }

    public void Spook()
    {
        spookTime = Time.time;

        if (state == State.Spooked)
            return;

        state = State.Spooked;
    }

    public void Attack()
    {
        if (state == State.Attacking)
            return;

        state = State.Attacking;
    }

    InsectBoid SelectTarget()
    {
        int randomTarget = Random.Range(0, InsectBoid.insects.Count - 1);
        return InsectBoid.insects[randomTarget];
    }

    private void OnDrawGizmosSelected()
    {
        if (state == State.Attacking)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        if(target!=null)
            Gizmos.DrawLine(target.transform.position,transform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, flightZoneRadius);
    }
}
