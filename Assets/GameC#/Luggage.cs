using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Luggage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetLugggage(string Luggage)
    {
        if (Luggage == this.gameObject.name)
        {
            Debug.Log("kta");
            Destroy(this.gameObject);

        }
    }
}
