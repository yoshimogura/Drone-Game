using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void ChangeScene(string sceneName)

    {
        Debug.Log("ButtonClick");
        SceneManager.LoadScene(sceneName);

    }
}
