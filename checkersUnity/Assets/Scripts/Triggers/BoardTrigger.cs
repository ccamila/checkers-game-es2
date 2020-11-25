using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    Piece pieceToUpdate;
    int[] currentPos;
    GameController gameController;
    public void OnMouseDown()
    {

        gameController = GameController.instance();
        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable());

        if (gameController.GetIsPieceClicked())
        {
            TableConstructor tableConstructor = TableConstructor.instance();

            if (gameObject.GetComponent<BoardPiece>().IsPlayable())
            {
                if (gameController.GetMandatoryEat() == true)
                {
                    gameController.SetNewBoardPosition(gameObject.GetComponent<BoardPiece>());
                    gameController.UpdateGameObjectBlockedDueMandatoryEat();
                    gameObject.GetComponent<BoardPiece>().SetPlayable();
                }
                else 
                {
                    
                    gameController.SetNewBoardPosition(gameObject.GetComponent<BoardPiece>());
                    gameController.UpdateGameObject();
                }
            }
        }
    }
}
