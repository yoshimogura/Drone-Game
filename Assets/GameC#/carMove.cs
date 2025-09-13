using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class carMove : MonoBehaviour
{
    public Transform target1; // 目的地（空オブジェクトなど）
    public Transform target2; // 目的地（空オブジェクトなど）
    public Transform target3; // 目的地（空オブジェクトなど）
    public Transform target4; // 目的地（空オブジェクトなど）
    private NavMeshAgent agent;
    int corner = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


    }

    void Update()
    {
        switch (corner)
        {
            case 1:
                agent.SetDestination(target1.position); // 目的地を設定
                Debug.Log("first");
                break;
            case 2:
                agent.SetDestination(target2.position);
                Debug.Log("second");
                break;
            case 3:
                agent.SetDestination(target3.position);
                break;
            case 4:
                agent.SetDestination(target4.position);
                break;
            default:
                break;

        }
        // 目的地に近づいたら停止する処理なども追加可能
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
             corner = (corner % 4) + 1; // 1〜4をループ
        }


    }
}
