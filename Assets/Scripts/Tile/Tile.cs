using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void PositionUpdateDelegate(Vector2 oldPosition, Vector2 newPosition);

public delegate void TileDelegate(Tile tile);

public class Tile : MonoBehaviour
{
    #region Properties

    public static event PositionUpdateDelegate OnPositionUpdate;

    public static event TileDelegate OnTileInstantiate;

    public static event Action OnMaxScoreReached;

    private Tile_Feedbacks feedbacks;

    public static TileDelegate OnValueUpdated;

    public bool IsMoving => feedbacks.IsMoving;

    private int value;

    public int Value
    {
        get => value;
        private set
        {
            this.value = value;
            OnValueUpdated(this);
            if (value == 2048)
            {
                OnMaxScoreReached();
            }
        }
    }

    public Vector2 Position
    {
        get => transform.position;
        private set
        {
            StartCoroutine(feedbacks.TranslateTo(transform.position, value));
        }
    }

    #endregion Properties

    #region System

    private void Start()
    {
        feedbacks = GetComponent<Tile_Feedbacks>();
    }

    private void OnEnable()
    {
        Position = transform.position;
        OnTileInstantiate(this);

        Value = 2;
    }

    public void Disable()
    {
        Value = 0;
    }

    #endregion System

    #region Main Methods

    public void Move(Vector2 direction)
    {
        Vector2 nextPosition = Position;

        while (true)
        {
            Vector2 currentPosition = nextPosition;

            if (!Playground.Instance.Tiles.ContainCellAt(nextPosition + direction))
                break;
            else if (Playground.Instance.Tiles.GetTileAt(nextPosition + direction).Value == 0)
            {
                nextPosition += direction;
                OnPositionUpdate(currentPosition, nextPosition);
            }
            else if (Playground.Instance.Tiles.GetTileAt(nextPosition + direction).Value != Value)
                break;
            else
            {
                Value *= 2;
                nextPosition += direction;
                OnPositionUpdate(currentPosition, nextPosition);
            }
        }

        Position = nextPosition;
    }

    #endregion Main Methods
}