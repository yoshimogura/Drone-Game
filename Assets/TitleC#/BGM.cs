using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGM : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        // 重複防止
        if (FindObjectsOfType<BGM>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Result") 
        {
            audioSource.Stop();
        }
    }

    

}
