using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICurrentTable
{
    public Board GetCurrentBoard();
    public void SetCurrentBoard(Board updatedBoard);
    public List<List<Piece>> GetPiecesPosition();
    public void SetPiecesPosition(List<List<Piece>> updatedPiecsositions);
}
