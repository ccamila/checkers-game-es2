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

        int row = 0; //checkersPiecesPositions.GetLength(0);
        int column = 0; //checkersPiecesPositions.GetLength(1);

        bool control = false;

        for (int i = 0; i < checkersPiecesPositions.Count; i++)
        {
/*            for (int j = 0; j < checkersPiecesPositions[0].Count; j++)
            {
                if(checkersPiecesPositions[i][j])
                Debug.Log(checkersPiecesPositions[i][j].gameObject.name + i + " vllv" + j +" gfgfgf");
            }*/

                Debug.Log(checkersPiecesPositions[i].Contains(gameObject.GetComponent<Piece>()));
        }
/*
        Debug.Log(checkersPiecesPositions.GetLength(0));
        Debug.Log(checkersPiecesPositions.GetLength(1));*/

/*        for (int i = 0; i < checkersPiecesPositions.GetLength(0); i++)
        {
            for (int j = 0; j < checkersPiecesPositions.GetLength(1); j++)
            {
                if(checkersPiecesPositions[i, j])
                Debug.Log(checkersPiecesPositions[i,j].gameObject.name + " " + i + " " + j);
            }

            //Debug.Log(checkersPiecesPositions[i].Contains(auxiliarPiece));
        }*/
/*        while (control)
        {
            if (checkersPiecesPositions[row, column] == gameObject.GetComponent<Piece>())
            {
                currentValue[0] = row;
                currentValue[1] = column;
                control = false;
            }
            if ((row == checkersPiecesPositions.GetLength(0) - 1) && column == checkersPiecesPositions.GetLength(1) - 1)
            {
                control = false;
            }

            if (column < checkersPiecesPositions.GetLength(1) - 1)
            {
                column++;
            }
            else 
            {
                column = 0;
                row++;
            }
        }*/

        Debug.Log(currentValue[0]);
        Debug.Log(currentValue[1]);
    }
}
