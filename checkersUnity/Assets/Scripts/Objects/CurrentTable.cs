using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTable : ICurrentTable
{
    private Board board;
    private List<List<Piece>> piecesPosition;

    public CurrentTable(Board boardGame, List<List<Piece>> piecesPositionGame)
    {
        board = boardGame;
        piecesPosition = piecesPositionGame;
    }

    public Board GetCurrentBoard()
    {
        return board;
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
}
