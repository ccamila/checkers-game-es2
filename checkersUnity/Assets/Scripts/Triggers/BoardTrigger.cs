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

        if (gameController.GetIsPieceClicked())
        {

            TableConstructor tableConstructor = TableConstructor.instance();
            Board board = tableConstructor.GetBoard();

            /*       for (int i = 0; i < playableArea.Count; i++)
                    {
                        for (int j = 0; j < playableArea[0].Count; j++)
                        {
                            Debug.Log(playableArea[i][j].gameObject.name + " ## ## ##" + playableArea[i][j].IsPlayable());

                        }
                        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable());
                    }*/

            /*        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable() + "fd");*/

            /*        for (int i = 0; i < board.GetBoardMatrix().Count; i++)
                    {
                        for (int j = 0; j < board.GetBoardMatrix()[0].tablePiecePosition.Count; j++)
                        {

                            Destroy(board.GetBoardMatrix()[i].tablePiecePosition[j].gameObject.GetComponent<MeshRenderer>().material = grayMaterial);
                        }
                    }*/

            if (gameObject.GetComponent<BoardPiece>().IsPlayable())
            {
/*                List<List<BoardPiece>> boardPieces = gameController.GetCurrentTable().GetCurretBoardPositions();
                bool controlPieces = true;
                int rowValues = 0;
                while (controlPieces)
                {
                    if (boardPieces[rowValues].Contains(gameObject.GetComponent<BoardPiece>()))
                    {
                        gameController.SetcurrentBoardPieceIndex(boardPieces[rowValues].IndexOf(gameObject.GetComponent<BoardPiece>()));
                        controlPieces = false;
                    }
                    rowValues++;
                    if (rowValues == boardPieces.Count)
                    {
                        controlPieces = false;
                    }
                }*/

                gameController.SetNewPOs(gameObject.GetComponent<BoardPiece>());
                gameController.updateGameobject();
                gameObject.GetComponent<BoardPiece>().SetPlayable();
            }
        }
    }
}
