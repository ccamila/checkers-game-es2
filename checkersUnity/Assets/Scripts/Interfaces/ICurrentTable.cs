using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICurrentTable
{
    Board GetCurrentBoard();
    void SetCurrentBoard(Board updatedBoard);
    List<List<BoardPiece>> GetCurretBoardSpacePositions();
    List<List<Piece>> GetPiecesPosition();

    void UpdatePiecesPosition(int row, int column, Piece piece);

}
