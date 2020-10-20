using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableConstructor : MonoBehaviour
{
    private static TableConstructor _instance;

    [SerializeField]
    private Transform centralPosition;
    [SerializeField]
    private Material blackMaterial;

    GameObject boardGameObject;
    private Board board;
    private int totalPieces;
    PiecesConstructor pieceConstructor;

    private List<List<BoardPiece>> playableArea;

    public static TableConstructor instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindObjectOfType<TableConstructor>();

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("TableConstructor");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<TableConstructor>();
                DontDestroyOnLoad(instanceObject);
            }
            else
                Debug.Log("Resource does not have a definition for TableConstructor");
        }
        return _instance;
    }


    private void Awake()
    {
        pieceConstructor = PiecesConstructor.instance();
        playableArea = new List<List<BoardPiece>>();

    }

    public void ConstructBoard()
    {
        BoardPiece boardSquare = Resources.Load<BoardPiece>("BoardPiece");
        board = Resources.Load<Board>("Board");
        int boardSize = board.GetSizeOfTable();
        int totalSize = boardSize * boardSize;
        int totalPieces = totalSize - 1;

        int columnValue = 0;
        bool blackTablePiece = false;
        float rowScale = boardSquare.transform.localScale.x;
        float columnScale = boardSquare.transform.localScale.y;
        int rowValue = 0;
        Quaternion originalRotation = boardSquare.transform.rotation;
        List<BoardPiece> rowToAddToBoard = new List<BoardPiece>();
        List<BoardPiece> playableAreaRow = new List<BoardPiece>();

        playableArea = new List<List<BoardPiece>>();

        while (totalPieces >= 0)
        {
            Vector3 positionSquare = new Vector3(rowScale * rowValue, columnScale * columnValue, 0);
            GameObject boardBlock = Instantiate(boardSquare.gameObject, positionSquare, originalRotation, centralPosition);


            if (columnValue % 2 == 0)
            {
                if (blackTablePiece == false)
                {
                    boardBlock.GetComponent<MeshRenderer>().material = blackMaterial;
                    blackTablePiece = true;
                    //board.SetBoardPiecePlayable(rowValue, columnValue);
                    
                    boardBlock.GetComponent<BoardPiece>().SetPlayable();
                }
                else
                {
                    blackTablePiece = false;
                }
            }
            else
            {
                if (blackTablePiece == false)
                {

                    blackTablePiece = true;
                }
                else
                {
                    boardBlock.GetComponent<MeshRenderer>().material = blackMaterial;
                    blackTablePiece = false;
                    //playableAreaRow.Add(boardBlock.GetComponent<BoardPiece>());
                    boardBlock.GetComponent<BoardPiece>().SetPlayable();
                }
            }
            rowValue++;
            playableAreaRow.Add(boardBlock.GetComponent<BoardPiece>());
            if (boardBlock.GetComponent<BoardPiece>() != null)
            {
                rowToAddToBoard.Add(boardBlock.GetComponent<BoardPiece>());
            }
            else 
            {
                Debug.Log("No BoardPiece Attached to GameObject " + boardBlock.name);
            }

            if (totalPieces % 8 == 0)
            {
               
                columnValue++;
                rowValue = 0;
                board.AddRowOfTablePieces(rowToAddToBoard);
                playableArea.Add(playableAreaRow);
                playableAreaRow = new List<BoardPiece>();
                rowToAddToBoard = new List<BoardPiece>();

            }
            totalPieces--;

        }

    }

    void BoardCounstructorOld()
    {
        BoardPiece boardSquare = Resources.Load<BoardPiece>("BoardPiece");

        boardGameObject = Resources.Load<GameObject>("Board");
        Instantiate(boardGameObject, centralPosition);
        board = boardGameObject.GetComponent<Board>();
        List<Board.BoardPiecesMatrix> boardPieces = board.GetBoardMatrix();
        List<BoardPiece> auxiliarBoardPiecesList = new List<BoardPiece>();
        if (boardPieces != null && boardPieces.Count > 0 && boardPieces[0].tablePiecePosition != null && boardPieces[0].tablePiecePosition.Count > 0)
        {
            totalPieces = boardPieces.Count * boardPieces[0].tablePiecePosition.Count;
            int columnValue = 0;
            int placeController = totalPieces - 1;
            int rowValue = 0;
            while (placeController > 0)
            {
                if (rowValue % 2 == 0)
                {
                    if (columnValue % 2 == 0)
                    {
                        board.GetBoardPiece(rowValue, columnValue).gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
                        board.SetBoardPiecePlayable(rowValue, columnValue);
                        auxiliarBoardPiecesList.Add(board.GetBoardPiece(rowValue, columnValue));
                    }
                }
                else
                {
                    if (columnValue % 2 != 0)
                    {
                        board.GetBoardPiece(rowValue, columnValue).gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
                        board.SetBoardPiecePlayable(rowValue, columnValue);
                        auxiliarBoardPiecesList.Add(board.GetBoardPiece(rowValue, columnValue));
                    }
                }
                placeController--;
                if (columnValue < boardPieces[0].tablePiecePosition.Count)
                {
                    columnValue++;
                }

                if (placeController % boardPieces.Count == 0)
                {
                    rowValue++;
                    columnValue = 0;
                    playableArea.Add(auxiliarBoardPiecesList);
                    auxiliarBoardPiecesList = new List<BoardPiece>();
                    
                }

            }
        }
        pieceConstructor.ConstructPieces();

        /*        for (int i = 0; i < playableArea.Count; i++)
                {
                    for (int j = 0; j < playableArea[0].Count; j++)
                    {
                        Debug.Log(playableArea[i][j].IsPlayable() + " playble area");
                    }
                }*/
    }


    public List<List<BoardPiece>> GetPlaybleArea()
    {
        if (playableArea.Count > 0)
        {
            /*            for (int i = 0; i < playableArea.Count; i++)
                        {
                            for (int j = 0; j < playableArea[0].Count; j++)
                            {
                                Debug.Log(playableArea[i][j].gameObject.name + " ## ## ##" + playableArea[i][j].IsPlayable());

                            }

                        }*/
            return playableArea;
        }
        return null;
    }

    public void SetPlaybleArea(int row, int column)
    {
        if (playableArea != null)
        {
            playableArea[row][column].SetPlayable();
        }
    }

    public Board GetBoard()
    {
        return board;
    }

}
