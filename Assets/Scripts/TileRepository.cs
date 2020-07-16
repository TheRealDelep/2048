using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileRepository
{
    #region Members

    private Matrix<Tile> matrix;

    // World space position of the bottom left corner
    private Vector2 Anchor { get; set; }

    private readonly GameObject owner;

    // Convert world space positions into matrix coordinates
    private Dictionary<Vector2, Tuple<int, int>> worldLocalPositions = new Dictionary<Vector2, Tuple<int, int>>();

    // Stores the item meant to be destroyed
    private List<Tile> DestructionPool = new List<Tile>();

    #endregion Members

    #region System

    public TileRepository(GameObject owner, float cellSize, int columnCount, int rowCount)
    {
        this.owner = owner;
        matrix = new Matrix<Tile>(columnCount, rowCount);

        Anchor = SetAnchor(owner.transform, cellSize);

        foreach (var cell in matrix.Cells)
        {
            Vector2 worldPosition = new Vector2(Anchor.x + cell.Item1 - 1, Anchor.y + cell.Item2 - 1);
            Tuple<int, int> localPosition = new Tuple<int, int>(cell.Item1, cell.Item2);
            worldLocalPositions.Add(worldPosition, localPosition);
        }

        Tile.OnTileInstantiate += InsertTile;
        Tile.OnPositionUpdate += UpdateMatrix;
    }

    private Vector2 SetAnchor(Transform owner, float cellSize)
    {
        Vector2 offset;

        offset.x = cellSize * 0.5f + (matrix.columnCount * 0.5f) - 1;
        offset.y = cellSize * 0.5f + (matrix.rowCount * 0.5f) - 1;

        return (Vector2)owner.position - offset;
    }

    public void ProcessDestroyPool()
    {
        if (DestructionPool.Count == 0)
            return;

        foreach (var item in DestructionPool)
        {
            MonoBehaviour.Destroy(item.gameObject);
        }

        DestructionPool.Clear();
    }

    #endregion System

    #region CRUD Functions

    #region Create

    public void InsertTile(Tile tile)
    {
        if (!ContainCellAt(tile.Position))
        {
            throw new ArgumentException($"Playground doesn't contains a cell at the position {tile.Position}");
        }

        Tuple<int, int> positionTuple = worldLocalPositions[tile.Position];
        matrix[positionTuple] = tile;
    }

    #endregion Create

    #region Read

    public Vector2[] GetCellsPositions()
        => worldLocalPositions.Keys.ToArray();

    public Tile[] GetAllTiles(ReadDirection direction = ReadDirection.FromBottom)
        => matrix[direction];

    public Tile GetTileAt(Vector2 position)
    {
        if (!ContainCellAt(position))
        {
            throw new ArgumentException($"Playground doesn't contains a cell at the position {position}");
        }

        Tuple<int, int> positionTuple = worldLocalPositions[position];
        return matrix[positionTuple];
    }

    public bool ContainCellAt(Vector2 position)
        => worldLocalPositions.ContainsKey(position);

    public bool ContainTileAt(Vector2 position)
    {
        if (!ContainCellAt(position))
        {
            throw new ArgumentException($"Playground doesn't contains a cell at the position {position}");
        }

        if (matrix.HasItemAt(worldLocalPositions[position]))
        {
            return matrix[worldLocalPositions[position]].Value != 0;
        }
        return false;
    }

    #endregion Read

    #region Update

    private void UpdateMatrix(Vector2 oldPosition, Vector2 newPosition)
    {
        if (!ContainCellAt(oldPosition))
        {
            throw new ArgumentException($"Playground doesn't contains a cell at the position {oldPosition}");
        }
        else if (!ContainCellAt(newPosition))
        {
            throw new ArgumentException($"Playground doesn't contains a cell at the position {newPosition}");
        }
        else
        {
            Tuple<int, int> oldPositionTuple = worldLocalPositions[oldPosition];

            Tuple<int, int> newPositionTuple = worldLocalPositions[newPosition];

            if (matrix.HasItemAt(newPositionTuple))
                RemoveTile(matrix[newPositionTuple]);

            Tile tile = GetTileAt(oldPosition);

            matrix[oldPositionTuple] = null;
            matrix[newPositionTuple] = tile;
        }
    }

    #endregion Update

    #region Delete

    // Soft Delete the item until all tiles positions are updated
    private void RemoveTile(Tile tile)
    {
        // Marks the item as unusable
        tile.Disable();
        // Place it to the the Destroy Pool to be destroyed later
        DestructionPool.Add(tile);
    }

    #endregion Delete

    #endregion CRUD Functions
}