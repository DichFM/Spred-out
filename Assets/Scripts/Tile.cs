using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum Role
{
    Free,
    Correct,
    Disabled,
}

public enum Owner
{
    Player1,
    Player2,
    Empty
}

[RequireComponent(typeof(TapGesture))]
public class Tile : MonoBehaviour
{
    [SerializeField] private TapGesture _tapGesture;
    [SerializeField] private float _hidingSpeed;
    [SerializeField] private float _showingSpeed;
    [SerializeField] private float _destroyDelay;
    [SerializeField] private Role _role = Role.Free;
    [SerializeField] private Owner _owner;
    [SerializeField] private bool _isLocked;
    [SerializeField] private GameObject _visual;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _hitFX;
    [SerializeField] private bool _isShowing;
    [SerializeField] private bool _isHiding;
    [SerializeField] private bool _isActive;
    [SerializeField] private Tile[] _neighborTiles;
    private float _timeFromLastTap;


    public Owner Owner
    {
        get => _owner;
        set => _owner = value;
    }

    public Role Role
    {
        get => _role;
        set => _role = value;
    }


    private void Awake()
    {
        _tapGesture = GetComponent<TapGesture>();
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _tapGesture.Tapped += Tapped;
    }

    private void OnDisable()
    {
        _tapGesture.Tapped -= Tapped;
    }

    private void Update()
    {
        if (_isHiding)
        {
            HideTileAnimation();
        }
        else if (_isShowing)
        {
            ShowTileAnimation();
        }
    }

    private void Tapped(object sender, EventArgs e)
    {
        if (_role == Role.Free)
        {
            GameManager.Instance.PlayWrongSound();
            return;
        }


        if (_role == Role.Correct)
        {
            Instantiate(_hitFX, transform.position, quaternion.identity);
            int scoretoPopup = 1;
            if (GameManager.Instance.GameMode == GameMode.Сapture)
            {
                scoretoPopup = GetFreeNeighborsNumber();
            }

            UI.Instance.CreateScorePopup(transform, scoretoPopup);
            GameManager.Instance.TapOnCorrectTile(this);
        }
    }

    private void HideTileAnimation()
    {
        transform.localScale = Vector3.MoveTowards(
            transform.localScale,
            Vector3.zero,
            Time.deltaTime * _hidingSpeed
        );

        if (transform.localScale.x <= 0.02f)
        {
            _visual.SetActive(false);
            _isHiding = false;
        }
    }

    public void ShowTileAnimation()
    {
        _visual.SetActive(true);
        transform.localScale =
            Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * _showingSpeed);


        if (transform.localScale == Vector3.one)
        {
            _isShowing = false;
        }
    }


    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Show()
    {
        _isShowing = true;
    }

    public void Hide()
    {
        _isHiding = true;
    }

    public void SetRole(Role role, Owner owner)
    {
        _role = role;
        _owner = owner;
    }


    public void CapturingNeighbors()
    {
        for (int i = 0; i < _neighborTiles.Length; i++)
        {
            var neighborTile = _neighborTiles[i];

            if (neighborTile.Owner != this.Owner)
            {
                neighborTile.Hide();
                neighborTile.Show();
                neighborTile.SetRole(Role.Correct, this.Owner);
                neighborTile.SetColor(_spriteRenderer.color);
            }
        }
    }


    public int GetFreeNeighborsNumber()
    {
        int value = 0;
        for (int i = 0; i < _neighborTiles.Length; i++)
        {
            if (_neighborTiles[i].Owner != this.Owner)
            {
                value++;
            }
        }

        return value;
    }


    public void Lock()
    {
        _isLocked = true;
    }

    public void Unlock()
    {
        _isLocked = false;
    }
}