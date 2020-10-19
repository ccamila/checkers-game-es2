using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTrigger : MonoBehaviour
{
    private int[] currentValue = new int[2];
    GameController gameController;
    public void OnMouseDown()
    {
        gameController = GameController.instance();

        TableConstructor tableConstructor = TableConstructor.instance();

        List<List<Piece>> checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();


        for (int i = 0; i < checkersPiecesPositions.Count; i++)
        {

            if (checkersPiecesPositions[i].Contains(gameObject.GetComponent<Piece>())) 
            {
                currentValue[0] = i;
                currentValue[1] = checkersPiecesPositions[i].IndexOf(gameObject.GetComponent<Piece>()) *2;
            }
        
                Debug.Log(checkersPiecesPositions[i].Contains(gameObject.GetComponent<Piece>()));
        }

        Debug.Log(currentValue[0]);
        Debug.Log(currentValue[1]);

        gameController.SetPiece(gameObject.GetComponent<Piece>());
        gameController.SetOldPOs(currentValue);
    }
}
