using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public enum GameMode
{
    Singleplayer,
    Multiplyaer
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float GameTimer;
    public Color RandomColor;
    public GameMode GameMode;
    public int FirstPlayerScore;
    public int SecondPlayerScore;
    private int _maxCorrectTilesSingleplayer = 9;
    private int _maxCorrectTilesMultiplayer = 6;
    private bool _isStarting;
    public Player Player1;
    public Player Player2;
    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private AudioSource _wrongSound;

    private float _roundTime = 20;


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
        
        

        StartCoroutine(StartGameDelay());
    }

    public IEnumerator StartGameDelay()
    {
        yield return new WaitForSeconds(2f);
        StartGame(GameMode);
    }

    public void StartGame(GameMode gameMode)
    {
        ScoreReset();
        TimeReset();
        UI.Instance.UIUpdate();
        GameTimer = _roundTime;
        _isStarting = true;

       
        TilesController.Instance.SetTileRole();
        TilesController.Instance.ShowAllTiles();
        TilesController.Instance.ShowSampleTile();


        RandomColor = ColorManager.Instance.GetRandomColor();
        TilesController.Instance.SetColorToSampleTile(RandomColor);

        if (gameMode == GameMode.Singleplayer)
        {
            TilesController.Instance.GenerateCorretTilesPlayer1(_maxCorrectTilesSingleplayer);
        }

        if (gameMode == GameMode.Multiplyaer)
        {
            TilesController.Instance.GenerateCorretTilesPlayer1(_maxCorrectTilesMultiplayer);
            TilesController.Instance.GenerateCorretTilesPlayer2(_maxCorrectTilesMultiplayer);
        }
    }


    private void Update()
    {
        if (_isStarting)
        {
            GameTimer -= Time.deltaTime;
            UI.Instance.UIUpdate();

            if (GameTimer <= 0)
            {
                StopGame();
            }
        }
    }

    private void TimeReset()
    {
        GameTimer = 0f;
    }

    private void ScoreReset()
    {
        FirstPlayerScore = 0;
        SecondPlayerScore = 0;
    }

    public void StopGame()
    {
        _isStarting = false;
        TilesController.Instance.DiactivateAllTiles();
    }


    public void TapOnCorrectTile(Tile tile)
    {
        if (tile.Owner == Owner.Empty)
            return;


            
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

        _correctSound.Play();
        tile.SetRole(Role.Free, tile.Owner);
        Debug.Log(tile.Owner);
        tile.SetColor(ColorManager.Instance.DefaultColor);


        if (Player1.CorrectСlicks >= 3)
        {
            Player1.CorrectСlicks = 0;
            TilesController.Instance.GenerateCorretTilesPlayer1(3);
        }

        if (Player2.CorrectСlicks >= 3)
        {
            Player2.CorrectСlicks = 0;
            TilesController.Instance.GenerateCorretTilesPlayer2(3);
        }
    }

    public void PlayWrongSound()
    {
        _wrongSound.Play();
    }
}