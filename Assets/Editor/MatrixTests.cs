using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;

public class MatrixTest
{
    private Matrix<int> matrix = new Matrix<int>(4, 4);

    public void InitializeMatrix()
    {
        int i = 0;

        for (int row = 1; row <= 4; row++)
        {
            for (int col = 1; col <= 4; col++)
            {
                matrix[new Tuple<int, int>(col, row)] = i;
                i++;
            }
        }
    }

    [Test]
    public void Test_Get_Set_On_Matrix()
    {
        InitializeMatrix();

        Assert.AreEqual(3, matrix[new Tuple<int, int>(4, 1)]);
    }

    [Test]
    public void Test_Reading_Down_Up()
    {
        InitializeMatrix();

        var sut = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        Assert.AreEqual(sut, matrix[ReadDirection.FromBottom]);
    }

    [Test]
    public void Test_Reading_Up_Down()
    {
        InitializeMatrix();

        var sut = new int[] { 12, 13, 14, 15, 8, 9, 10, 11, 4, 5, 6, 7, 0, 1, 2, 3 };

        Assert.AreEqual(sut, matrix[ReadDirection.FromTop]);
    }

    [Test]
    public void Test_Reading_Left_Right()
    {
        InitializeMatrix();
        var sut = new int[] { 0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15 };

        Assert.AreEqual(sut, matrix[ReadDirection.FromLeft]);
    }

    [Test]
    public void Test_Reading_Right_Left()
    {
        InitializeMatrix();
        var sut = new int[] { 3, 7, 11, 15, 2, 6, 10, 14, 1, 5, 9, 13, 0, 4, 8, 12 };

        Assert.AreEqual(sut, matrix[ReadDirection.FromRight]);
    }
}