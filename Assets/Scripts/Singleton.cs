using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    internal static Singleton Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            //return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
