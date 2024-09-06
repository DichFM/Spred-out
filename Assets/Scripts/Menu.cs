using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private TapGesture _singleMode;
    [SerializeField] private TapGesture _multiplayerMode;
    [SerializeField] private AudioSource _fxSound;


    private void OnEnable()
    {
        _singleMode.Tapped += StartSingle;
        _multiplayerMode.Tapped += StartMulti;
    }


    private void OnDisable()
    {
        _singleMode.Tapped -= StartSingle;
        _multiplayerMode.Tapped -= StartMulti;
    }


    private void StartMulti(object sender, EventArgs e)
    {
        _fxSound.Play();
        StartCoroutine(LoadScene(1));
        
    }
    
    private void StartSingle(object sender, EventArgs e)
    {
        _fxSound.Play();
        StartCoroutine(LoadScene(2));
    }


    private IEnumerator LoadScene(int id)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(id);
    }


}