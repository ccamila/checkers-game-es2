using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPieces 
{
    void Walk();
    bool  GetIsBlack();
    void SetBlackColor(bool color); // 0 for white, 1 for black
    bool GetKingStatus();
    void SetKing(bool isThePieceKing);
    bool GetIsUp();
    void SetIsUp(bool isThePieceUpInTable);
    bool GetIsAvaiableToEat();
    void SetIsAvaiableToEat(bool isAvaiableToEatValue);
}
