using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        Invoke(nameof(Destroy), 0.5f);
    }

    public void SetValue(int value)
    {
        _text.text = $"+ {value.ToString()}";
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}