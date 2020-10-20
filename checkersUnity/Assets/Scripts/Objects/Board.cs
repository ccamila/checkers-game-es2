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

    [Serializable]
    public class BoardPiecesMatrix
    {
        [SerializeField]
        public List<BoardPiece> tablePiecePosition;
    }

    [SerializeField]
    public List<BoardPiecesMatrix> BoardPieces;

    private List<List<BoardPiece>> tableMatrix;

    private List<List<Piece>> checkersPiecesPositionsList;

    public void OnChangeBehaviour()
    {
        if (boardClassic_8x8 == true)
        {
            boardLarger_10x10 = false;
        }
        else
        {
            boardLarger_10x10 = true;
        }
    }

    public int GetSizeOfTable()
    {
        return tableSize;
    }

    public void SetSizeOfTable(int newSize)
    {
        tableSize = newSize;
    }

    public List<BoardPiecesMatrix> GetBoardMatrix()
    {
        return BoardPieces;
    }

    public void SetBoardPiecePlayable(int row, int column)
    {
        BoardPieces[row].tablePiecePosition[column].SetPlayable();
    }

    public BoardPiece GetBoardPiece(int row, int column)
    {
        return BoardPieces[row].tablePiecePosition[column];
    }

    public List<List<Piece>> GetPiecesPositionList()
    {
        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = StartEmpityList();
        }
        return checkersPiecesPositionsList;
    }

    public void SetPiecesPositionList(int row, int column, Piece piece)
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
        if (tableMatrix == null)
        {
            tableMatrix = new List<List<BoardPiece>>(0);
        }
        tableMatrix.Add(newTableRow);
    }

}



