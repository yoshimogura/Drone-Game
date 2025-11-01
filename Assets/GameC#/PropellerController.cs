using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    public float currentSpeed = 0f;
    float maxSpeed = 5000f;
    public float acceleration = 500f;  // 毎秒どれだけ加速するか
    float deceleration = 1800f;
    
    public enum State { Idle, TakingOff, Flying, Landing }
    public State currentState = State.Idle;
    private Quaternion initialRotation;

    // void Start()
    // {
    //     initialRotation = transform.rotation;
    // }
    void Update()
    {
        
        switch (currentState)
        {
            case State.
            TakingOff:

                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
                break;
            case State.Flying:
                currentSpeed = maxSpeed;
                break;
            case State.Landing:
                currentSpeed = maxSpeed - 3200;
                // Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0f);
                // transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, 100f * Time.deltaTime);

                break;
            case State.Idle:
                currentSpeed = 0f;
                // transform.rotation = initialRotation; // 最終的にしっかり揃える

                break;
        }

        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}
