using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);//
    }

}
