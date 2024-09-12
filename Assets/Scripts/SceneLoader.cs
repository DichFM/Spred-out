using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SaveLoadSettings _saveLoadSettings;
    private int _sceneID = 1;


    private void Start()
    {
        _saveLoadSettings.LoadData();
        _sceneID = _saveLoadSettings.getDataIntById(0);
        SceneManager.LoadScene(_sceneID);
    }
}