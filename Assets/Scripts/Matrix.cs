using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Matrix<T>
{
    public readonly int columnCount;
    public readonly int rowCount;
    public T[] elements;

    public List<Tuple<int, int>> Cells
    {
        get
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    result.Add(new Tuple<int, int>(j, i));
                }
            }
            return result;
        }
    }

    public Matrix(int rows, int columns)
    {
        columnCount = columns;
        rowCount = rows;
        elements = new T[columns * rows];
    }

    public T this[Tuple<int, int> coordinates]
    {
        get
        {
            int flattened = ((coordinates.Item2 - 1) * columnCount) + coordinates.Item1 - 1;
            return elements[flattened];
        }
        set
        {
            int flattened = ((coordinates.Item2 - 1) * columnCount) + coordinates.Item1 - 1;
            elements[flattened] = value;
        }
    }

    public T[] this[ReadDirection direction]
    {
        // Reads the matrix in four different directions
        get
        {
            int elementCount = elements.Where(x => x != null).Count();

            T[] result = new T[elementCount];
            int resultIndex = 0;

            int revertedIterator = 1;
            int actualRow;

            // Iterates over rows
            for (int row = rowCount; row >= 1; row--)
            {
                // Invert the values of rows if the should be readed in a decreasing order
                if (direction == ReadDirection.FromBottom || direction == ReadDirection.FromLeft)
                {
                    actualRow = revertedIterator;
                    revertedIterator++;
                }
                else
                {
                    actualRow = row;
                }

                // Then Iterates over columns
                for (int col = 1; col <= columnCount; col++)
                {
                    // Invert column and rows if the matrix should be readed horizontaly
                    if (direction == ReadDirection.FromLeft || direction == ReadDirection.FromRight)
                    {
                        if (this[new Tuple<int, int>(actualRow, col)] != null)
                        {
                            result[resultIndex] = this[new Tuple<int, int>(actualRow, col)];
                            resultIndex++;
                        }
                    }
                    else
                    {
                        if (this[new Tuple<int, int>(col, actualRow)] != null)
                        {
                            result[resultIndex] = this[new Tuple<int, int>(col, actualRow)];
                            resultIndex++;
                        }
                    }
                }
            }
            return result;
        }
    }

    public bool HasItemAt(Tuple<int, int> coordinates)
        => this[coordinates] != null;
}

public enum ReadDirection
{
    FromTop, FromBottom, FromLeft, FromRight
}