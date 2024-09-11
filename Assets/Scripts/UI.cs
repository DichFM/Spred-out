using System;
using TMPro;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UI : MonoBehaviour
{
    public static UI Instance;

    [SerializeField] private TextMeshProUGUI _firstPlayerScoreText;
    [SerializeField] private TextMeshProUGUI _secondPlayerScoreText;
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private GameObject _scorePopup;
    [SerializeField] private GameObject _uiParent;
    [SerializeField] private TapGesture _menuButton;
    [SerializeField] private TapGesture _restartButton;
    [SerializeField] private GameObject _endGameUI;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private GameObject _endGameButtons;
    [SerializeField] private GameObject _helpScreen;
    [SerializeField] private GameObject[] _headUI;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        _menuButton.Tapped += LoadMenu;
        _restartButton.Tapped += RestartGame;
    }

    private void RestartGame(object sender, EventArgs e)
    {
        GameManager.Instance.RestartGame();
    }

    public void ShowHeadUI()
    {
        for (int i = 0; i < _headUI.Length; i++)
        {
            _headUI[i].SetActive(true);
        }
    }

    public void HideHeadUI()
    {
        for (int i = 0; i < _headUI.Length; i++)
        {
            _headUI[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        _menuButton.Tapped -= LoadMenu;
        _restartButton.Tapped -= RestartGame;
    }

    private void LoadMenu(object sender, EventArgs e)
    {
        SceneManager.LoadScene(0);
    }

    public void UIUpdate()
    {
        _firstPlayerScoreText.text = GameManager.Instance.Player1.Score.ToString();
        _secondPlayerScoreText.text = GameManager.Instance.Player2.Score.ToString();
        _gameTimerText.text = GameManager.Instance.GameTimer.ToString("00");
    }

    public void CreateScorePopup(Transform transform, int value)
    {
        var newPopup = Instantiate(_scorePopup, transform.position, Quaternion.identity, _uiParent.transform);
        newPopup.GetComponent<Popup>().SetValue(value);
    }

    public void ShowHelpText()
    {
        _helpScreen.SetActive(true);
    }

    public void HideHelpText()
    {
        _helpScreen.SetActive(false);
    }

    public void ShowEndGameUI(GameMode gameMode, int winner)
    {
        Invoke(nameof(ShowEndGameButtons), 3f);
        _endGameUI.SetActive(true);
        if (gameMode == GameMode.Multiplyaer | gameMode == GameMode.Domination)
        {
            switch (winner)
            {
                case 1:
                    _winnerText.text = "PLAYER 1 WON";
                    break;
                case 2:
                    _winnerText.text = "PLAYER 2 WON";
                    break;
                case 3:
                    _winnerText.text = "DRAW";
                    break;
            }
        }
        else
        {
            _winnerText.text = $"YOUR SCORE: {GameManager.Instance.Player1.Score}";
        }
    }

    public void ShowEndGameButtons()
    {
        _endGameButtons.SetActive(true);
    }

    public void HideEndGameUI()
    {
        _endGameButtons.SetActive(false);
        _endGameUI.SetActive(false);
    }
}