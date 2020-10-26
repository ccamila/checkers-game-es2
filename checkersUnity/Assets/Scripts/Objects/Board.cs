using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
    [SerializeField]
    private bool boardClassic_8x8 = true;

    [SerializeField]
    private bool boardLarger_10x10 = false;

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

    public void SetBoardPiecePlayable(int row, int column)
    {
        boardPpiecesMatrix[row][column].SetPlayable();
    }

    public BoardPiece GetBoardPiece(int row, int column)
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



