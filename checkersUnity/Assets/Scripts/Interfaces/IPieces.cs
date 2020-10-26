using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPieces 
{
    void Walk();
    bool  IsBlack();
    void SetBlackColor(bool color); // 0 for white, 1 for black
    bool GetIsKing();
    void SetIsKing(bool isThePieceKing);
    bool GetIsUp();
    void SetIsUp(bool isThePieceUpInTable);
}
