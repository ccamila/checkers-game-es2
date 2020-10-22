using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoard
{

    int GetSizeOfTable();
    void SetSizeOfTable(int newSize);
    List<List<BoardPiece>> GetBoardMatrix();
    void SetBoardPiecePlayable(int row, int column);
    BoardPiece GetBoardPiece(int row, int column);
    List<List<Piece>> GetPiecesPositionList();
    void UpdatePiecesPositionList(int row, int column, Piece piece);
    List<List<Piece>> StartEmpityList();
    void AddRowOfTablePieces(List<BoardPiece> newTableRow);


}
