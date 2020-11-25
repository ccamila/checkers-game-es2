using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{

    [SerializeField]
    private int tableSize = 8;

    private List<List<BoardPiece>> boardPpiecesMatrix;

    private List<List<Piece>> checkersPiecesPositionsList;

    public int GetSizeOfTable()
    {
        return tableSize;
    }

    public void SetSizeOfTable(int newSize)
    {
        tableSize = newSize;
    }

    public List<List<BoardPiece>> GetBoardMatrix()
    {
        return boardPpiecesMatrix;
    }

    public void SetBoardTilePlayable(int row, int column)
    {
  //      Debug.Log("updattng pos " + row + " " + column + " from " + boardPpiecesMatrix[row][column].IsPlayable() + " to " + !boardPpiecesMatrix[row][column].IsPlayable());
        boardPpiecesMatrix[row][column].SetPlayable();
    }

    public void SetBoardTilePlayable(int row, int column, bool playbleState)
    {
//        Debug.Log("updating pos " + row + " " + column + " from " + boardPpiecesMatrix[row][column].IsPlayable() + " to " + playbleState);
        boardPpiecesMatrix[row][column].SetPlayable(playbleState);
    }

    public BoardPiece GetBoardSpace(int row, int column)
    {
        return boardPpiecesMatrix[row][column];
    }

    public List<List<Piece>> GetPiecesPositionList()
    {
        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = StartEmpityList();
        }
        return checkersPiecesPositionsList;
    }

    public void UpdatePiecesPositionList(int row, int column, Piece piece)
    {

        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = StartEmpityList();
        }


        checkersPiecesPositionsList[row][column] = piece;

    }

    private List<List<Piece>> StartEmpityList()
    {
        List<List<Piece>> emptyList = new List<List<Piece>>(); 
        for (int j = 0; j < tableSize; j++)
        {
            List<Piece>  auxiliarList = new List<Piece>();
            for (int i = 0; i < tableSize; i++)
            {
                auxiliarList.Add(null);
            }

            emptyList.Add(auxiliarList);
        }

        return emptyList;
    }
    public void AddRowOfTablePieces(List<BoardPiece> newTableRow)
    {
        if (boardPpiecesMatrix == null)
        {
            boardPpiecesMatrix = new List<List<BoardPiece>>(0);
        }
        boardPpiecesMatrix.Add(newTableRow);
    }

}



