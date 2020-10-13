using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    public void OnMouseDown()
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
        Debug.Log(gameObject.GetComponent<MeshRenderer>().material.name + "fd");
        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable() + "fd");
        gameObject.name = "DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD";

        /*        Debug.Log(gameObject.GetComponent<BoardPiece>().IsPlayable() + "fd");*/

/*        for (int i = 0; i < board.GetBoardMatrix().Count; i++)
        {
            for (int j = 0; j < board.GetBoardMatrix()[0].tablePiecePosition.Count; j++)
            {

                Destroy(board.GetBoardMatrix()[i].tablePiecePosition[j].gameObject.GetComponent<MeshRenderer>().material = grayMaterial);
            }
        }*/
        Debug.Log(gameObject.GetComponent<MeshRenderer>().material.name + "fd");
    }
}
