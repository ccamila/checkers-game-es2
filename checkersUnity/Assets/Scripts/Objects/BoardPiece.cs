using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece :MonoBehaviour, IBoardPiece
{

    public GameObject table;

    private bool isPlayable = false;
    [SerializeField]
    private Transform centralPosition;
    public bool IsPlayable()
    {
        return isPlayable;
    }

    public void SetPlayable()
    {
/*        Debug.Log("Changing " + isPlayable);*/
        isPlayable = !isPlayable;
    /*    Debug.Log("Changed " + isPlayable);*/
    }

    public Transform GetCentralPosition()
    {
        return centralPosition;
    }

    public BoardPiece GetBoardPiece()
    {
        return this;
    }
}
