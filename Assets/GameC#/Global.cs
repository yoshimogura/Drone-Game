using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    public static void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void Sound()
    {
        audioSource.PlayOneShot(audioSource.clip);

    }
}
