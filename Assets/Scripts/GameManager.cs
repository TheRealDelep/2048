using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Playground playground;

    public static GameManager Instance;

    private bool IsUpdatingPlayground;

    public static Action OnPlaygroundUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        playground.InstantiateNewTile();
        playground.InstantiateNewTile();
    }
}