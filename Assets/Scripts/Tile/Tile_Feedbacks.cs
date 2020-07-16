using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile_Feedbacks : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [SerializeField] private float translationSpeed;

    public TextMeshPro Text;
    public static Dictionary<int, Color> valueColors { get; set; }

    [SerializeField] public Color[] Colors = new Color[11];

    private void Start()
    {
        valueColors = new Dictionary<int, Color>();
        for (int i = 0; i < Colors.Length; i++)
        {
            valueColors.Add((int)Math.Pow(2, i), Colors[i]);
        }
    }

    private void OnEnable()
    {
        Text = GetComponentInChildren<TextMeshPro>();
    }

    public IEnumerator TranslateTo(Vector2 origin, Vector2 destination)
    {
        IsMoving = true;
        Vector2 direction = (destination - origin).normalized;

        float distance = Vector2.Distance(transform.position, destination);

        while (distance >= 0)
        {
            distance -= translationSpeed * Time.deltaTime;
            transform.Translate(direction * translationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destination;
        IsMoving = false;
    }
}