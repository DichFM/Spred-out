
using System;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance;
    
    [SerializeField] private TextMeshProUGUI _firstPlayerScoreText;
    [SerializeField] private TextMeshProUGUI _secondPlayerScoreText;
    [SerializeField] private TextMeshProUGUI _gameTimerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UIUpdate()
    {
        _firstPlayerScoreText.text = GameManager.Instance.Player1.Score.ToString();
        _secondPlayerScoreText.text = GameManager.Instance.Player2.Score.ToString();
        _gameTimerText.text =GameManager.Instance.GameTimer.ToString("00");
    }
}
