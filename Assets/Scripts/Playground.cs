using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = System.Random;

public class Playground : MonoBehaviour
{
    [SerializeField]
    private GameObject TilePrefab;

    private Rnd random = new Rnd();

    public TileRepository Tiles { get; private set; }

    public static Playground Instance;

    public static Action OnInputProcessed;

    private bool noMovingTile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);

        Tiles = new TileRepository(gameObject, 1, 4, 4);
        InputManager.OnInputReceived += MoveTiles;
    }

    public void InstantiateNewTile(ReadDirection direction = ReadDirection.FromBottom)
    {
        Vector2 position;

        do
        {
            position = Tiles.GetCellsPositions()[random.Next(0, 16)];
        } while (Tiles.ContainTileAt(position));

        Instantiate(TilePrefab, position, Quaternion.identity);
    }

    public void MoveTiles(ReadDirection direction)
    {
        noMovingTile = true;

        Vector2 directionVector = Vector2.zero;

        switch (direction)
        {
            case ReadDirection.FromTop:
                directionVector = Vector2.up;
                break;

            case ReadDirection.FromBottom:
                directionVector = Vector2.down;
                break;

            case ReadDirection.FromLeft:
                directionVector = Vector2.left;
                break;

            case ReadDirection.FromRight:
                directionVector = Vector2.right;
                break;

            default:
                directionVector = Vector2.up;
                break;
        }

        Tile[] tiles = Tiles.GetAllTiles(direction);
        StartCoroutine(ObserveTiles(tiles));

        for (int i = 0; i < tiles.Count(); i++)
        {
            if (tiles[i].Value > 0)
            {
                Vector2 lastPosition = tiles[i].Position;
                tiles[i].Move(directionVector);
                if (tiles[i].Position != lastPosition)
                {
                    noMovingTile = false;
                }
            }
            Debug.Log(noMovingTile);
        }
    }

    private IEnumerator ObserveTiles(Tile[] tiles)
    {
        yield return new WaitUntil(() => tiles.Where(x => !x.IsMoving).Count() == 0);

        Tiles.ProcessDestroyPool();
        if (!noMovingTile)
        {
            InstantiateNewTile();
        }
        OnInputProcessed();
    }
}