using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTrigger : MonoBehaviour
{
    private int row, column;
    GameController gameController;
    public void OnMouseDown()
    {
        gameController = GameController.instance();
        if (gameController.GetIsPieceClicked())
        {
            if (gameController.GetClickedPiece() == gameObject)
            {
                gameController.SetIsPieceClicked();
                gameController.SetClickedPiece(null);
            }
            else 
            {
                ClickedBehaviour();
            }
        }
        else
        {
            ClickedBehaviour();
        }
    }
    private void ClickedBehaviour()
    {
        gameController.SetIsPieceClicked();
        
        gameController.SetClickedPiece(gameObject);

        TableConstructor tableConstructor = TableConstructor.instance();

        List<List<Piece>> checkersPiecesPositions = gameController.GetCurrentTable().GetPiecesPosition();

        bool checkObjectController = true;
        int indexOfList = 0;

        while (checkObjectController)
        {

            if (checkersPiecesPositions[indexOfList].Contains(gameObject.GetComponent<Piece>()))
            {
                Debug.Log(indexOfList + " index of current piece");
                row = indexOfList;
                column = checkersPiecesPositions[indexOfList].IndexOf(gameObject.GetComponent<Piece>());
                checkObjectController = false;
            }
            else if (indexOfList == checkersPiecesPositions.Count - 1)
            {
                checkObjectController = false;
            }
            else
            {

                indexOfList++;
            }
        }

        gameController.SetPiece(gameObject.GetComponent<Piece>());
        gameController.SetOldPOs(row, column);
    }
}
