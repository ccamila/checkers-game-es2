using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoard
{
    void ConstructBoard();
    void OnChangeBehaviour();
    Piece[,] GetPiecesPosition();
    void SetPiecesPosition(int row, int column, Piece piece);

}
