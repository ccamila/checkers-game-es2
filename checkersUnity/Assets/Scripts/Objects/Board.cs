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
        public List<BoardPiece> tablePiecePosition;
    }

    [SerializeField]
    public List<BoardPiecesMatrix> BoardPieces;

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
        //checkersPiecesPositions = new Piece[BoardPieces.Length, BoardPieces.Length / 2];
        if (boardClassic_8x8 == boardLarger_10x10)
        {
            boardClassic_8x8 = true;
            boardLarger_10x10 = false;
        }
        //ConstructBoard();
    }

    public List<BoardPiecesMatrix> GetBoardMatrix()
    {
        return BoardPieces;
    }

    public void SetBoardPiecePlayable(int row, int column)
    {
        BoardPieces[row].tablePiecePosition[column].SetPlayable();
    }

    public BoardPiece GetBoardPiece(int row, int column)
    {
        return BoardPieces[row].tablePiecePosition[column];
    }

    public List<List<Piece>> GetPiecesPositionList()
    {
        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = StartEmpityList();
        }
        return checkersPiecesPositionsList;
    }

    public void SetPiecesPositionList(int row, int column, Piece piece)
    {
        if (checkersPiecesPositionsList == null)
        {
            checkersPiecesPositionsList = StartEmpityList();
        }
        checkersPiecesPositionsList[row][column] = piece;
    }

    private List<List<Piece>> StartEmpityList()
    {

        List<List<Piece>> emptyList = new List<List<Piece>>(); 
        for (int j = 0; j < BoardPieces.Count; j++)
        {
            List<Piece>  auxiliarList = new List<Piece>();
            for (int i = 0; i < BoardPieces.Count / 2; i++)
            {
                auxiliarList.Add(null);
            }

            emptyList.Add(auxiliarList);
        }

        return emptyList;
    }
}



