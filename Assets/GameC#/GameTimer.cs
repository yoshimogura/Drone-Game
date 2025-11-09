using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameTimer Instance;

    private float startTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sceneをまたいでも保持
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }
    void Start()
    {
        GameTimer.Instance.StartTimer();
    }



}
