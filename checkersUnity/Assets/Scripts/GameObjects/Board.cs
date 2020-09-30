using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
    [SerializeField]
    private bool boardClassic_8x8 = true;

    [SerializeField]
    private bool boardLarger_10x10 = false;

    [SerializeField]
    private GameObject WhiteSquare;

    [SerializeField]
    private GameObject BlackSquare;

    [SerializeField]
    private Transform centralPlaceholder;


    public void OnChangeBehaviour()
    {
        if (boardClassic_8x8 == true)
        {
            boardLarger_10x10 = false;
        }
        else
        {
            boardLarger_10x10 = true;
        }
    }

    public void ConstructBoard()
    {
        GameObject squareGame = Instantiate(WhiteSquare, centralPlaceholder, true);
        Vector3 positionSquare = new Vector3(centralPlaceholder.position.x, centralPlaceholder.position.y, 0);
        WhiteSquare.transform.position = positionSquare;

        int columnValue = 0;
        bool black = true;
        float rowScale = squareGame.transform.localScale.x;
        float columnScale = squareGame.transform.localScale.y;
        int rowValue = 1;
        Quaternion originalRotation = squareGame.transform.rotation;
        if (boardClassic_8x8 == true)
        {
            int totalPieces = 62;
            while (totalPieces >= 0)
            {
                if (columnValue % 2 == 0)
                {
                    if (black == true)
                    {
                        positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                        if (BlackSquare)
                        {
                            Instantiate(BlackSquare, positionSquare, originalRotation);
                        }
                        black = false;
                    }
                    else
                    {
                        positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                        if (WhiteSquare)
                        {
                            Instantiate(WhiteSquare, positionSquare, originalRotation);
                        }
                        black = true;
                    }
                }
                else
                {
                    if (black == true)
                    {

                        positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                        if (WhiteSquare)
                        {
                            Instantiate(WhiteSquare, positionSquare, originalRotation);
                        }
                        black = false;

                    }
                    else
                    {
                        positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                        if (BlackSquare)
                        {
                            Instantiate(BlackSquare, positionSquare, originalRotation);
                        }
                        black = true;

                    }

                }
                rowValue++;
                if (totalPieces % 8 == 0)
                {
                    columnValue++;
                    rowValue = 0;

                }
                totalPieces--;

            }


        }

    }

    void Awake()
    {
        if (boardClassic_8x8 == boardLarger_10x10)
        {
            boardClassic_8x8 = true;
            boardLarger_10x10 = false;
        }
        ConstructBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
