using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeInertia : MonoBehaviour
{
    public int sampleCount;

    Vector2[] positionRegister;
    float[] posTimeRegister;
    private int positionSamplesTaken;

    public float accelerationSmooth;
    public float maxAcceleration;
    public Vector2 acceleration { get; private set; }
    Vector2 refAcceleration;

    // Update is called once per frame
    void Update()
    {
        Vector2 newAcceleration;
        LinearAcceleration(out newAcceleration, transform.position, sampleCount);

        acceleration = Vector2.SmoothDamp(acceleration, newAcceleration, ref refAcceleration, accelerationSmooth, Mathf.Infinity, Time.deltaTime);
        acceleration = Vector2.ClampMagnitude(acceleration, maxAcceleration);
    }

    public bool LinearAcceleration(out Vector2 vector, Vector2 position, int samples)
    {

        Vector2 averageSpeedChange = Vector2.zero;
        vector = Vector2.zero;
        Vector2 deltaDistance;
        float deltaTime;
        Vector2 speedA;
        Vector2 speedB;

        if (samples < 3)
        {

            samples = 3;
        }

        if (positionRegister == null)
        {

            positionRegister = new Vector2[samples];
            posTimeRegister = new float[samples];
        }

        for (int i = 0; i < positionRegister.Length - 1; i++)
        {

            positionRegister[i] = positionRegister[i + 1];
            posTimeRegister[i] = posTimeRegister[i + 1];
        }
        positionRegister[positionRegister.Length - 1] = position;
        posTimeRegister[posTimeRegister.Length - 1] = Time.time;

        positionSamplesTaken++;

        if (positionSamplesTaken >= samples)
        {

            for (int i = 0; i < positionRegister.Length - 2; i++)
            {

                deltaDistance = positionRegister[i + 1] - positionRegister[i];
                deltaTime = posTimeRegister[i + 1] - posTimeRegister[i];

                if (deltaTime == 0)
                {

                    return false;
                }

                speedA = deltaDistance / deltaTime;
                deltaDistance = positionRegister[i + 2] - positionRegister[i + 1];
                deltaTime = posTimeRegister[i + 2] - posTimeRegister[i + 1];

                if (deltaTime == 0)
                {

                    return false;
                }

                speedB = deltaDistance / deltaTime;

                averageSpeedChange += speedB - speedA;
            }

            averageSpeedChange /= positionRegister.Length - 2;

            float deltaTimeTotal = posTimeRegister[posTimeRegister.Length - 1] - posTimeRegister[0];

            vector = averageSpeedChange / deltaTimeTotal;

            return true;
        }

        else
        {

            return false;
        }
    }
}
