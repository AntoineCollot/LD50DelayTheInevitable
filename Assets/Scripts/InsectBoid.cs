using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Left, Right }
public class InsectBoid : MonoBehaviour
{
    public float playerProximityZoneRadius = 5;
    public float flockingMaxDistance = 2;

    public float maxVelocityBase = 0.2f;
    public float maxVelocityPlayerNear = 3.5f;
    Vector3 refPosition;
    public float velocitySmooth = 0.2f;

    Vector3 targetVelocity;
    Vector3 velocity;

    [Header("Direction")]
    Direction dir;
    float lastTurnTime;
    const float TURN_INTERVAL = 1;

    [Header("Cohesion")]
    public float weightCohesionBase = 0.5f;
    public float weightCohesionPlayerNear = 5;

    [Header("Separation")]
    public float weightSeparationBase = 2;
    public float weightSeparationPlayerNear = 0;

    [Header("Lights")]
    public float weightLightAttraction = 3;

    [Header("Player")]
    public float weightPlayerAttraction = 6;
    Transform player;
    public static List<InsectBoid> insects = new List<InsectBoid>();

    [Header("FX")]
    public GameObject fireFX;
    public GameObject bloodFX;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        insects.Add(this);
    }

    private void OnDisable()
    {
        insects.Remove(this);
    }

    private void OnDestroy()
    {
        if(Application.isPlaying)
            Destroy(Instantiate(bloodFX, transform.position, Quaternion.identity, null),2);
    }

    float P(float x)
    {
        return (1 / Mathf.PI) * Mathf.Atan((playerProximityZoneRadius - x) / 0.3f) + 0.5f;
    }

    float CombineWeight(float mult1, float mult2, float x)
    {
        return mult1 * (1 + P(x) * mult2);
    }

    public static float Inv(float x, float s)
    {
        float value = x / s + Mathf.Epsilon;
        return 1 / (value * value);
    }


    Vector3 RuleCohesion()
    {
        //Find the average position
        Vector3 averagePosition = Vector3.zero;
        int averageCount = 0;

        foreach (InsectBoid insect in insects)
        {
            if (insect == this || Vector3.Distance(insect.transform.position, transform.position) > flockingMaxDistance)
                continue;
            averageCount++;
            averagePosition += insect.transform.position;
        }

        if (averageCount == 0)
            return Vector3.zero;

        averagePosition /= averageCount;

        return (averagePosition - transform.position) / (averagePosition - transform.position).magnitude;
    }

    Vector3 RuleSeparation()
    {
        Vector3 separationVector = Vector3.zero;

        foreach (InsectBoid insect in insects)
        {
            if (insect == this)
                continue;

            if (Vector3.Distance(insect.transform.position, transform.position) > flockingMaxDistance || Vector3.Distance(insect.transform.position, transform.position) < 0.001f)
                continue;

            Vector3 insectToInsect = transform.position - insect.transform.position;
            float magnitude = insectToInsect.magnitude;
            separationVector += (insectToInsect / magnitude) * Inv(magnitude, 1);
        }

        //Separate with player too
        if (Vector3.Distance(player.position, transform.position) < flockingMaxDistance && Vector3.Distance(player.position, transform.position) > 0.001f)
        {
            Vector3 playerToInsect = transform.position - player.transform.position;
            separationVector += (playerToInsect / playerToInsect.magnitude) * Inv(playerToInsect.magnitude, 1);
        }

        return separationVector;
    }

    //Vector3 RuleAlignment()
    //{
    //    Vector3 alignmentVector = Vector3.zero;
    //    int nearbySheepCount = 0;

    //    foreach (SheepBoid sheep in SheepHerd.Instance.sheeps)
    //    {
    //        if (sheep == this)
    //            continue;

    //        if (Vector3.Distance(sheep.transform.position, transform.position) <= alignementZoneRadius)
    //        {
    //            alignmentVector += sheep.velocity;
    //            nearbySheepCount++;
    //        }
    //    }

    //    if (nearbySheepCount > 0)
    //        return alignmentVector / nearbySheepCount;
    //    else
    //        return Vector3.zero;
    //}

    //Vector3 RuleEscape()
    //{
    //    Vector3 predatorToSheep = transform.position - player.position;
    //    float predatorToSheepMagnitude = predatorToSheep.magnitude;
    //    //Change 10 to 2
    //    return (predatorToSheep / predatorToSheepMagnitude) * Inv(predatorToSheepMagnitude, 2);
    //}

    const float SWITCH_PLAYER_ATTRACTION_DIST = 0.75f;
    Vector3 RuleAttractedByPlayer()
    {
        Vector3 insectToPlayer = player.position - transform.position;
        if (insectToPlayer.magnitude < SWITCH_PLAYER_ATTRACTION_DIST)
            return Vector3.zero;
        return insectToPlayer.normalized * Inv(insectToPlayer.magnitude, 5);
    }

    Vector3 RuleAttractedByLight()
    {

        LightAttraction closestLight = LightAttraction.GetMostAttractiveLight(transform.position);
        Vector3 insectToLight = closestLight.transform.position - transform.position;
        return insectToLight.normalized;
    }

    Vector3 ApplyRules()
    {
        float distanceToPlayer = (transform.position - player.position).magnitude;

        Vector3 v = Vector3.zero;
        v += RuleCohesion() * CombineWeight(weightCohesionBase, weightCohesionPlayerNear, distanceToPlayer);
        v += RuleSeparation() * CombineWeight(weightSeparationBase, weightSeparationPlayerNear, distanceToPlayer);
        v += RuleAttractedByPlayer() * weightPlayerAttraction;
        v += RuleAttractedByLight() * weightLightAttraction;

        //Debug.DrawRay(transform.position, RuleCohesion() * CombineWeight(weightCohesionBase, weightCohesionPlayerNear, distanceToPlayer), Color.green);
        //Debug.DrawRay(transform.position, RuleSeparation() * CombineWeight(weightSeparationBase, weightSeparationPlayerNear, distanceToPlayer), Color.black);
        //Debug.DrawRay(transform.position, RuleAttractedByPlayer() * weightPlayerAttraction, Color.red);

        return v;
    }

    void Update()
    {
        targetVelocity = ApplyRules();

        Turn();
    }

    void Turn()
    {
        if (Time.time < lastTurnTime + TURN_INTERVAL)
            return;
        if (targetVelocity.x > 0 && dir != Direction.Right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            dir = Direction.Right;
            lastTurnTime = Time.time;
        }
        else if (targetVelocity.x < 0 && dir != Direction.Left)
        {
            transform.localScale = Vector3.one;
            dir = Direction.Left;
            lastTurnTime = Time.time;
        }
    }

    void Move()
    {
        float distanceToPlayer = (transform.position - player.position).magnitude;

        float currentMaxVelocity = Mathf.Lerp(maxVelocityBase, maxVelocityPlayerNear, 1 - (distanceToPlayer / playerProximityZoneRadius));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, currentMaxVelocity);
        targetVelocity.z = 0;
        Debug.DrawRay(transform.position, targetVelocity, Color.blue);

        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref refPosition, velocitySmooth);
        velocity.z = 0;

        if (velocity.magnitude > 0)
            transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(Instantiate(fireFX, transform.position, Quaternion.identity, null),2);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerProximityZoneRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, flockingMaxDistance);

        Gizmos.color = Color.red;
        if (LightAttraction.lights != null && LightAttraction.lights.Count > 0)
            Gizmos.DrawLine(transform.position, LightAttraction.GetMostAttractiveLight(transform.position).transform.position);
    }
#endif
    void LateUpdate()
    {
        Move();
    }
}
