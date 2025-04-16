using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public  class SingletonTest : MonoBehaviour
{
    public static SingletonTest instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
