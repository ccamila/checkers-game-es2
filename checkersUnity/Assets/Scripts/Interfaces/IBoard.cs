using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoard
{

    int GetSizeOfTable();
    void SetSizeOfTable(int newSize);
    List<List<BoardPiece>> GetBoardMatrix();
    void SetBoardTilePlayable(int row, int column);
    void SetBoardTilePlayable(int row, int column, bool playbleState);
    BoardPiece GetBoardSpace(int row, int column);
    List<List<Piece>> GetPiecesPositionList();
    void UpdatePiecesPositionList(int row, int column, Piece piece);
    void AddRowOfTablePieces(List<BoardPiece> newTableRow);


}
