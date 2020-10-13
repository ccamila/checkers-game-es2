using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
    [SerializeField]
    private bool boardClassic_8x8 = true;

    [SerializeField]
    private bool boardLarger_10x10 = false;

    [Serializable]
    public class BoardPiecesMatrix
    {
        [SerializeField]
        public BoardPiece[] tablePiecePosition;

    }

    [SerializeField]

    public BoardPiecesMatrix[] BoardPieces;

    private Piece[,] checkersPiecesPositions;
    private List<List<Piece>> checkersPiecesPositionsList;

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
        /*    
                GameObject squareGame = Instantiate(BlackSquare, centralPlaceholder, true);
                Vector3 positionSquare = new Vector3(centralPlaceholder.position.x, centralPlaceholder.position.y, 0);
                BlackSquare.transform.position = positionSquare;

                int columnValue = 0;
                bool blackTablePiece = false;
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
                            if (blackTablePiece == false)
                            {
                                positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                                if (WhiteSquare)
                                {
                                    Instantiate(WhiteSquare, positionSquare, originalRotation, centralPlaceholder);
                                }
                                blackTablePiece = true;
                            }
                            else
                            {
                                positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                                if (BlackSquare)
                                {
                                    GameObject squareTable = Instantiate(WhiteSquare, positionSquare, originalRotation, centralPlaceholder);
                                    //squareTable.GetComponent<MeshRenderer>().material = colorMaterial[rowValue % 2];
                                    //Instantiate(BlackSquare, positionSquare, originalRotation, centralPlaceholder);
                                }
                                blackTablePiece = false;
                            }
                        }
                        else
                        {
                            if (blackTablePiece == false)
                            {

                                positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                                if (BlackSquare)
                                {
                                    Debug.Log(rowValue);
                                    Debug.Log(columnValue);
                                    //GameObject squareTable = Instantiate(WhiteSquare, positionSquare, originalRotation, centralPlaceholder);
                                    //squareTable.GetComponent<MeshRenderer>().material = colorMaterial[(rowValue % 2) -1];
                                }
                                blackTablePiece = true;

                            }
                            else
                            {
                                positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
                                if (WhiteSquare)
                                {
                                    Instantiate(WhiteSquare, positionSquare, originalRotation, centralPlaceholder);
                                }
                                blackTablePiece = false;

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

                Piece pieceBuilder = new Piece();
                pieceBuilder.BuildPieces();
                //concludedTableBuild = true; */



    }


    void Awake()
    {
        //checkersPiecesPositionsList = startEmpityList();
        checkersPiecesPositions = new Piece[BoardPieces.Length, BoardPieces.Length / 2];
        if (boardClassic_8x8 == boardLarger_10x10)
        {
            boardClassic_8x8 = true;
            boardLarger_10x10 = false;
        }
        //ConstructBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public BoardPiecesMatrix[] GetBoardMatrix()
    {
        return BoardPieces;
    }

    public Piece[,] GetPiecesPosition()
    {
        if (checkersPiecesPositions == null)
        {
            checkersPiecesPositions = new Piece[BoardPieces.Length, BoardPieces.Length / 2];
        }
        return checkersPiecesPositions;
    }

    public void SetPiecesPosition(int row, int column, Piece piece)
    {
        if (checkersPiecesPositions == null)
        {
            checkersPiecesPositions = new Piece[BoardPieces.Length, BoardPieces.Length / 2];
        }

        checkersPiecesPositions[row, column] = piece;

        /*        for (int i = 0; i < checkersPiecesPositions.GetLength(0); i++)
                {
                    for (int j = 0; j < checkersPiecesPositions.GetLength(1); j++)
                    {
                        Debug.Log(checkersPiecesPositions[i,j] + " " + i + " " + j);
                    }
                }*/
        //return checkersPiecesPositions;

    }

    public List<List<Piece>> GetPiecesPositionList()
    {
        if (checkersPiecesPositionsList == null)
        {
            Debug.Log("NOOOO list");
            checkersPiecesPositionsList = startEmpityList();

        }

/*        for (int i = 0; i < checkersPiecesPositionsList.Count; i++)
        {
            for (int j = 0; j < checkersPiecesPositionsList[0].Count; j++)
            {
                Debug.Log(checkersPiecesPositionsList[i][j] + " " + i + " " + j + " Debugging bug");
            }
        }*/
        return checkersPiecesPositionsList;
    }

    public void SetPiecesPositionList(int row, int column, Piece piece)
    {
/*        Debug.Log("Receiving " + (piece.gameObject.name + "at row" + row, "and colummun" + column));*/
        if (checkersPiecesPositionsList == null)
        {
/*            Debug.Log("noo");*/
            checkersPiecesPositionsList = startEmpityList();

        }
/*        Debug.Log(checkersPiecesPositionsList.Count + " size total row");
        Debug.Log(checkersPiecesPositionsList[0].Count + "size total columns");


        Debug.Log("Saving NOW " + (piece.gameObject.name + "at row" + row, "and colummun" + column));*/

        checkersPiecesPositionsList[row][column] = piece;

/*        Debug.Log("Final value " + checkersPiecesPositionsList[row][column].gameObject.name);*/

/*        for (int i = 0; i < checkersPiecesPositionsList.Count; i++)
        {
            for (int j = 0; j < checkersPiecesPositionsList[0].Count; j++)
            {
                Debug.Log(checkersPiecesPositionsList[i][j] + " " + i + " " + j + " Debugging bug");
            }
        }*/
    }

    private List<List<Piece>> startEmpityList()
    {

        List<List<Piece>> emptyList = new List<List<Piece>>(); //new Piece[BoardPieces.Length, BoardPieces.Length / 2];
        for (int j = 0; j < BoardPieces.Length; j++)
        {
            List<Piece>  auxiliarList = new List<Piece>();
            for (int i = 0; i < BoardPieces.Length / 2; i++)
            {
                auxiliarList.Add(null);
            }

            emptyList.Add(auxiliarList);
        }

        return emptyList;
    }


    /*public void SetPiecesPositionList(List<List<Piece>> pieceList)
    {
        //Debug.Log("adding piece" + piece.gameObject.name);
*//*        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = startEmpityList();

        }*//*

        checkersPiecesPositionsList = pieceList;
        //Debug.Log(checkersPiecesPositionsList[row][column].name + "added now");
        //Debug.Log(checkersPiecesPositionsList[row][column].gameObject.name + " " + row + " " + column);

        for (int i = 0; i < checkersPiecesPositionsList.Count; i++)
        {
            for (int j = 0; j < checkersPiecesPositionsList[0].Count; j++)
            {

                Debug.Log(checkersPiecesPositionsList[i][j].gameObject.name + " " + i + " " + j + "Not testable problem he");
            }


        }

    }*/

}



