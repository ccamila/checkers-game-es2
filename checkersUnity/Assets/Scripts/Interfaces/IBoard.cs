using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoard
{

    int GetSizeOfTable();
    void SetSizeOfTable(int newSize);
    List<List<BoardPiece>> GetBoardMatrix();
    void SetBoardSpacePlayable(int row, int column);
    BoardPiece GetBoardSpace(int row, int column);
    List<List<Piece>> GetPiecesPositionList();
    void UpdatePiecesPositionList(int row, int column, Piece piece);
    void AddRowOfTablePieces(List<BoardPiece> newTableRow);


}
