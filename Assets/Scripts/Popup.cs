using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(Destroy), 0.5f);
    }


    private void Destroy()
    {
        Destroy(gameObject);
    }
}