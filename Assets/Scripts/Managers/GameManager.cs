using System;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public enum GameMode
{
    Singleplayer,
    Multiplyaer,
    Domination
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float GameTimer;
    public Color RandomColor;
    public Color RandomColor2;
    public GameMode GameMode;
    private int _maxCorrectTilesSingleplayer = 9;
    private int _maxCorrectTilesMultiplayer = 6;
    private bool _isStarting;
    public Player Player1;
    public Player Player2;

    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private AudioSource _wrongSound;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _timerSound;
    [SerializeField] private GameObject _tilesGO;
    [SerializeField] private TapGesture _startButton;
    [SerializeField] private AudioSource _buttonFxSound;
    private float _timeFromLastTap;
    private float _timerFromLastWorngSound;
    private float _roundTime = 6;
    private bool _timerFlag;
    private const float SoundFadeSeconds = 1.2f;


    private void OnEnable()
    {
        _startButton.Tapped += StartButton;
    }

    private void OnDisable()
    {
        _startButton.Tapped -= StartButton;
    }

    private void StartButton(object sender, EventArgs e)
    {
        _buttonFxSound.Play();
        StartCoroutine(StartGameDelay());
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;


        Player1 = new Player();
        Player2 = new Player();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameMode = GameMode.Multiplyaer;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameMode = GameMode.Singleplayer;
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            GameMode = GameMode.Domination;
        }
        
    }
    private void Start()
    {
        UI.Instance.HideHeadUI();
        StartCoroutine(FadeIn(0, _menuMusic));
    }
    public IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(1f);
        UI.Instance.HideHelpText();
        StartGame(GameMode);
    }

    public void RestartGame()
    {
        StartGame(GameMode);
    }

    public void StartGame(GameMode gameMode)
    {
        _tilesGO.SetActive(true);
        ScoreReset();
        TimeReset();

        StartCoroutine(FadeOut(0, _menuMusic));


        StartCoroutine(FadeIn(1, _gameMusic));
        //_gameMusic.Play();
        // _menuMusic.Stop();

        UI.Instance.HideEndGameUI();
        UI.Instance.ShowHeadUI();
        UI.Instance.UIUpdate();
        GameTimer = _roundTime;
        _isStarting = true;

        RandomColor = ColorManager.Instance.GetRandomColor();
        do
        {
            RandomColor2 = ColorManager.Instance.GetRandomColor();
        } while (RandomColor2 == RandomColor);

        TilesManager.Instance.ShowAllTiles();
        TilesManager.Instance.SetTileRole();


        TilesManager.Instance.ShowSampleTile();


        TilesManager.Instance.SetColorToSampleTile(RandomColor);

        if (gameMode == GameMode.Singleplayer)
        {
            TilesManager.Instance.GenerateCorretTilesPlayer1(_maxCorrectTilesSingleplayer);
        }

        if (gameMode == GameMode.Multiplyaer)
        {
            TilesManager.Instance.GenerateCorretTilesPlayer1(_maxCorrectTilesMultiplayer);
            TilesManager.Instance.GenerateCorretTilesPlayer2(_maxCorrectTilesMultiplayer);
        }
    }


    private void Update()
    {
        if (_isStarting)
        {
            GameTimer -= Time.deltaTime;
            if (GameMode == GameMode.Domination)
            {
                Player1.Score = TilesManager.Instance.GetOccupiedTilesNumber(Owner.Player1);
                Player2.Score = TilesManager.Instance.GetOccupiedTilesNumber(Owner.Player2);
            }

            UI.Instance.UIUpdate();

            if (GameTimer <= 5 & _timerFlag == false)
            {
                _timerSound.Play();
                _timerFlag = true;
            }

            if (GameTimer <= 0)
            {
                StopGame();
            }

            _timerFromLastWorngSound += Time.deltaTime;
        }


        _timeFromLastTap += Time.deltaTime;

        if (GameMode == GameMode.Domination)
        {
            if (_timeFromLastTap > 3f)
            {
                _timeFromLastTap = 0;
                TilesManager.Instance.ResetRandomTiles();
            }
        }
    }



    private void TimeReset()
    {
        GameTimer = 0f;
    }

    private void ScoreReset()
    {
        Player1.ScoreReset();
        Player2.ScoreReset();
    }

    public void StopGame()
    {
        _tilesGO.SetActive(false);
        StartCoroutine(FadeOut(0, _gameMusic));


        StartCoroutine(FadeIn(1, _menuMusic));

        _timerFlag = false;
        _timerSound.Stop();
        _isStarting = false;
        TilesManager.Instance.DiactivateAllTiles();
        TilesManager.Instance.HideAllTiles();
        int winner;

        if (Player1.Score > Player2.Score)
        {
            winner = 1;
        }
        else if (Player2.Score > Player1.Score)
        {
            winner = 2;
        }
        else
        {
            winner = 3;
        }

        UI.Instance.ShowEndGameUI(GameMode, winner);
    }


    public void TapOnCorrectTile(Tile tile)
    {
        if (tile.Owner == Owner.Empty)
            return;

        _correctSound.Play();


        if (GameMode == GameMode.Domination)
        {
            tile.CapturingNeighbors();
        }
        else
        {
            tile.Hide();
            if (tile.Owner == Owner.Player1)
            {
                Player1.Score++;
                Player1.CorrectСlicks++;
            }

            if (tile.Owner == Owner.Player2)
            {
                Player2.Score++;
                Player2.CorrectСlicks++;
            }


            tile.Hide();
            tile.SetRole(Role.Free, tile.Owner);
            tile.SetColor(ColorManager.Instance.DefaultColor);


            if (Player1.CorrectСlicks >= 3)
            {
                Player1.CorrectСlicks = 0;
                TilesManager.Instance.GenerateCorretTilesPlayer1(3);
            }

            if (Player2.CorrectСlicks >= 3)
            {
                Player2.CorrectСlicks = 0;
                TilesManager.Instance.GenerateCorretTilesPlayer2(3);
            }

            tile.Show();
        }
    }

    public void PlayWrongSound()
    {
        if (_timerFromLastWorngSound >= 0.5f)
        {
            _wrongSound.Play();
            _timerFromLastWorngSound = 0;
        }
    }


    private IEnumerator FadeOut(float delay, AudioSource audioSource)
    {
        yield return new WaitForSeconds(delay);
        float timeElapsed = 0;

        while (audioSource.volume > 0)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timeElapsed / SoundFadeSeconds);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeIn(float delay, AudioSource audioSource)
    {
        yield return new WaitForSeconds(delay);
        audioSource.volume = 0;
        audioSource.Play();
        float timeElapsed = 0;

        while (audioSource.volume < 1)
        {
            audioSource.volume = Mathf.Lerp(0, 1, timeElapsed / SoundFadeSeconds);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}