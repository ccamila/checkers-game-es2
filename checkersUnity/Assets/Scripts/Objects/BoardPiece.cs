using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece :MonoBehaviour, IBoardPiece
{
    private bool isPlayable = false;
    [SerializeField]
    private Transform centralPosition;
    public bool IsPlayable()
    {
        return isPlayable;
    }

    public void SetPlayable()
    {
        isPlayable = !isPlayable;
    }

    public Transform GetCentralPosition()
    {
        return centralPosition;
    }
}
