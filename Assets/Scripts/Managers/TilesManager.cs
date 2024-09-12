using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance;

    [SerializeField] private Tile _sampleTile;
    [SerializeField] private Tile[] _centerTiles;
    [SerializeField] private Tile[] _firstPlayerTiles;
    [SerializeField] private Tile[] _secondPlayerTiles;
    [SerializeField] private Tile[] _allTiles;
    [SerializeField] private Tile[] _dominationModeStartTiles;
    [SerializeField] private List<Tile> _freeTilesPlayer1 = new List<Tile>();
    [SerializeField] private List<Tile> _freeTilesPlayer2 = new List<Tile>();
    [SerializeField] private List<Tile> _occupiedTilesPlayer1 = new List<Tile>();
    [SerializeField] private List<Tile> _occupiedTilesPlayer2 = new List<Tile>();
    private int _maxCorrectClicks = 3;
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    

    public void SetTileRole()
    {
        if (GameManager.Instance.GameMode == GameMode.Singleplayer)
        {
            for (int i = 0; i < _allTiles.Length; i++)
            {
                _allTiles[i].SetRole(Role.Free, Owner.Player1);
            }
        }

        if (GameManager.Instance.GameMode == GameMode.Multiplyaer)
        {
            for (int i = 0; i < _firstPlayerTiles.Length; i++)
            {
                _firstPlayerTiles[i].SetRole(Role.Free, Owner.Player1);
            }

            for (int i = 0; i < _secondPlayerTiles.Length; i++)
            {
                _secondPlayerTiles[i].SetRole(Role.Free, Owner.Player2);
            }

            for (int i = 0; i < _centerTiles.Length; i++)
            {
                _centerTiles[i].SetRole(Role.Disabled, Owner.Empty);
            }
        }

        if (GameManager.Instance.GameMode == GameMode.Сapture)
        {
            for (int i = 0; i < _allTiles.Length; i++)
            {
                _allTiles[i].SetRole(Role.Free, Owner.Empty);
            }

            _dominationModeStartTiles[0].SetRole(Role.Correct, Owner.Player1);
            _dominationModeStartTiles[0].SetColor(GameManager.Instance.RandomColor);
            _dominationModeStartTiles[0].Lock();

            _dominationModeStartTiles[1].SetRole(Role.Correct, Owner.Player2);
            _dominationModeStartTiles[1].SetColor(GameManager.Instance.RandomColor2);
            _dominationModeStartTiles[1].Lock();
        }
    }

    public int GetOccupiedTilesNumber(Owner owner)
    {
        int number = 0;
        
        for (int i = 0; i < _allTiles.Length; i++)
        {
            if (_allTiles[i].Owner == owner)
            {
                number++;
            }
        }

        return number;
    }

    public void ResetRandomTiles()
    {
        Tile randomTile;
        UpdateOccupiedTiles();
        if (_occupiedTilesPlayer1.Count > 2)
        {
            randomTile = _occupiedTilesPlayer1[Random.Range(0, _occupiedTilesPlayer1.Count)];
            randomTile.Hide();
            randomTile.SetRole(Role.Free, Owner.Empty);
            randomTile.SetColor(ColorManager.Instance.DefaultColor);
            randomTile.Show();
        }


        UpdateOccupiedTiles();
        if (_occupiedTilesPlayer2.Count > 2)
        {
            randomTile = _occupiedTilesPlayer2[Random.Range(0, _occupiedTilesPlayer2.Count)];
            randomTile.Hide();
            randomTile.SetRole(Role.Free, Owner.Empty);
            randomTile.SetColor(ColorManager.Instance.DefaultColor);
            randomTile.Show();
        }
    }
    
    

    private void UpdateOccupiedTiles()
    {
        _occupiedTilesPlayer1.Clear();
        _occupiedTilesPlayer2.Clear();
        for (int i = 0; i < _allTiles.Length; i++)
        {
            if (_allTiles[i].Owner == Owner.Player1)
            {
                _occupiedTilesPlayer1.Add(_allTiles[i]);
            }

            if (_allTiles[i].Owner == Owner.Player2)
            {
                _occupiedTilesPlayer2.Add(_allTiles[i]);
            }
        }
    }

    private void UpdateFreeTilesList()
    {
        _freeTilesPlayer1.Clear();
        _freeTilesPlayer2.Clear();

        for (int i = 0; i < _allTiles.Length; i++)
        {
            if (_allTiles[i].Role == Role.Free)
            {
                switch (_allTiles[i].Owner)
                {
                    case Owner.Player1:
                        _freeTilesPlayer1.Add(_allTiles[i]);
                        break;
                    case Owner.Player2:
                        _freeTilesPlayer2.Add(_allTiles[i]);
                        break;
                }
            }
        }
    }

    public void ShowSampleTile()
    {
        _sampleTile.Show();
    }


    public void ShowAllTiles()
    {
        AllTilesResetColor();
        for (int i = 0; i < _allTiles.Length; i++)
        {
            if (_allTiles[i].Role != Role.Disabled)
            {
                _allTiles[i].Show();
            }
        }
    }

    public void HideAllTiles()
    {
        _sampleTile.Hide();
        for (int i = 0; i < _allTiles.Length; i++)
        {
            _allTiles[i].Hide();
        }
    }

    public void GenerateCorretTilesPlayer1(int amount)
    {
        UpdateFreeTilesList();
        for (int i = 0; i < amount; i++)
        {
            Tile newCorrectTile = GetRandomTile(_freeTilesPlayer1);
            newCorrectTile.SetRole(Role.Correct, Owner.Player1);
            newCorrectTile.SetColor(GameManager.Instance.RandomColor);
        }
    }

    public void GenerateCorretTilesPlayer2(int amount)
    {
        UpdateFreeTilesList();
        for (int i = 0; i < amount; i++)
        {
            Tile newCorrectTile = GetRandomTile(_freeTilesPlayer2);
            newCorrectTile.SetRole(Role.Correct, Owner.Player2);
            newCorrectTile.SetColor(GameManager.Instance.RandomColor);
        }
    }


    private Tile GetRandomTile(List<Tile> tileList)
    {
        Tile randomTile = tileList[Random.Range(0, tileList.Count)];
        tileList.Remove(randomTile);
        return randomTile;
    }

    public void SetColorToSampleTile(Color color)
    {
        _sampleTile.SetColor(color);
    }


    public void DiactivateAllTiles()
    {
        for (int i = 0; i < _allTiles.Length; i++)
        {
            _allTiles[i].Owner = Owner.Empty;
        }
    }

    public void AllTilesResetColor()
    {
        for (int i = 0; i < _allTiles.Length; i++)
        {
            _allTiles[i].SetColor(ColorManager.Instance.DefaultColor);
        }
    }
}