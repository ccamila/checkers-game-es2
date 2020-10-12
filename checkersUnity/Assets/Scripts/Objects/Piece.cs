using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPieces
{
    private bool isBlack;
    public bool IsBlack()
    {
        return isBlack;
    }

    public void SetBlackColor(bool isColorBalck)
    {
        isBlack = isColorBalck;
    }

    public void Walk()
    {
        throw new System.NotImplementedException();
    }
}
