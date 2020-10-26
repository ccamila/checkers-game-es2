using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTable : ICurrentTable
{
    private Board board;
    private List<List<Piece>> piecesPosition;
    private bool isPiecePressed;
    public CurrentTable(Board boardGame, List<List<Piece>> piecesPositionGame)
    {
        board = boardGame;
        piecesPosition = piecesPositionGame;
    }

    public Board GetCurrentBoard()
    {
        return board;
    }

    public List<List<BoardPiece>> GetCurretBoardPositions()
    {
        return board.GetBoardMatrix();
    }


    public void SetCurrentBoard(Board updatedBoard)
    {
        board = updatedBoard;
    }


    public List<List<Piece>> GetPiecesPosition()
    {
        return piecesPosition;
    }
    public void SetPiecesPosition(List<List<Piece>> updatedPiecsositions)
    {
        piecesPosition = updatedPiecsositions;
    }

    public void UpdatePiecesPosition(int row, int column, Piece piece)
    {
        piecesPosition[row][column] = piece;
    }

}
