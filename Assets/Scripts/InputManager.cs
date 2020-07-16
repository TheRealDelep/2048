using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public static event OnInputDelegate OnInputReceived;

    private bool IsInputLocked { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        IsInputLocked = false;

        OnInputReceived += (ReadDirection) => IsInputLocked = true;
        Playground.OnInputProcessed += () => IsInputLocked = false;
    }

    private void Update()
    {
        if (IsInputLocked)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInputReceived(ReadDirection.FromTop);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputReceived(ReadDirection.FromBottom);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnInputReceived(ReadDirection.FromLeft);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputReceived(ReadDirection.FromRight);
        }
    }
}

public delegate void OnInputDelegate(ReadDirection direction = ReadDirection.FromBottom);