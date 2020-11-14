using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoardPiece
{
    bool IsPlayable();

    void SetPlayable();

    void SetPlayable(bool playbleState);

    Transform GetCentralPosition();

    BoardPiece GetBoardPiece();

}
