using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    public void OnMouseDown()
    {
        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable());

    }
}
