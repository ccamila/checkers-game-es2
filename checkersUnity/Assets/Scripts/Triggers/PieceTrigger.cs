using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTrigger : MonoBehaviour
{
    private int[] currentValue = new int[2];
    public void OnMouseDown()
    { 

        TableConstructor tableConstructor = TableConstructor.instance();

        List<List<Piece>> checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();

        int row = 0; 
        int column = 0; 

        bool control = false;

        for (int i = 0; i < checkersPiecesPositions.Count; i++)
        {

                Debug.Log(checkersPiecesPositions[i].Contains(gameObject.GetComponent<Piece>()));
        }

        Debug.Log(currentValue[0]);
        Debug.Log(currentValue[1]);
    }
}
