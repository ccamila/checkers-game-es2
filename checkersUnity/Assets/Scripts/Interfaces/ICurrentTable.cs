using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICurrentTable
{
    Board GetCurrentBoard();
    void SetCurrentBoard(Board updatedBoard);
    List<List<Piece>> GetPiecesPosition();
    void SetPiecesPosition(List<List<Piece>> updatedPiecsositions);
    void UpdatePiecesPosition(int row, int column, Piece piece);

}
