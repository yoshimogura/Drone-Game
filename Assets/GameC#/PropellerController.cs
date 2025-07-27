using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    public float currentSpeed = 0f;
    public float maxSpeed = 1000f;
    public float acceleration = 500f;  // 毎秒どれだけ加速するか
    public float deceleration = 700f;
    
    public enum State { Idle, TakingOff, Flying, Landing }
    public State currentState = State.Idle;

    void Update()
    {
        switch (currentState)
        {
            case State.TakingOff:
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
                break;
            case State.Flying:
                currentSpeed = maxSpeed;
                break;
            case State.Landing:
                currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0f);
                break;
            case State.Idle:
                currentSpeed = 0f;
                break;
        }

        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}
