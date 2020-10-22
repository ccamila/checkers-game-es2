using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoard
{
/*    void ConstructBoard();*/
    void OnChangeBehaviour();
    List<List<Piece>> GetPiecesPositionList();
    void UpdatePiecesPositionList(int row, int column, Piece piece);

}
