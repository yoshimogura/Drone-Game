using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public GameObject targetParent;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (Transform child in targetParent.transform)
        {
            if (child.GetComponent<MeshFilter>() != null)
            {
                MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();
                collider.convex = false; 
            }
        }

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
