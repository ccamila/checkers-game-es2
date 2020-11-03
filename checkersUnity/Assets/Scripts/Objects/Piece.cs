using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPieces
{
    private bool isBlack;
    private bool isKing;
    private bool isUp;
    private bool isAvaiableToEat;

    public bool GetIsKing()
    {
        return isKing;
    }

    public bool GetIsBlack()
    {
        return isBlack;
    }

    public void SetBlackColor(bool isColorBalck)
    {
        isBlack = isColorBalck;
    }

    public void SetIsKing(bool isThePieceKing)
    {
        isKing = isThePieceKing;
    }

    public void Walk()
    {
        throw new System.NotImplementedException();
    }

    public bool GetIsUp()
    {
        return isUp;
    }

    public void SetIsUp(bool isThePieceUpInTable)
    {
        isUp = isThePieceUpInTable;
    }

    public bool GetIsAvaiableToEat()
    {
        return isAvaiableToEat;
    }

    public void SetIsAvaiableToEat(bool isAvaiableToEatValue)
    {
        isAvaiableToEat = isAvaiableToEatValue;
    }
}
