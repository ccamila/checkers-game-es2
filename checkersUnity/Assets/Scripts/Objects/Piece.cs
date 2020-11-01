using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPieces
{
    private bool isBlack;
    private bool isKing;
    private bool isUp;

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

}
