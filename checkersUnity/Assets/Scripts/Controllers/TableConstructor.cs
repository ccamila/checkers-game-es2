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
    PiecesConstructor pc;


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
        pc = PiecesConstructor.instance();
        playableArea = new List<List<BoardPiece>>();
        BoardCounstructor();

    }

    void BoardCounstructor()
    {
        boardGameObject = Resources.Load<GameObject>("Board");
        Instantiate(boardGameObject, centralPosition);
        board = boardGameObject.GetComponent<Board>();
        var boardPieces = board.GetBoardMatrix();
        List<BoardPiece> auxiliarBoardPiecesList = new List<BoardPiece>();
        if (boardPieces != null && boardPieces.Length > 0 && boardPieces[0].tablePiecePosition != null && boardPieces[0].tablePiecePosition.Length > 0)
        {
            totalPieces = boardPieces.Length * boardPieces[0].tablePiecePosition.Length;
            int columnValue = 0;
            int placeController = totalPieces - 1;
            int rowValue = 0;
            while (placeController > 0)
            {
                if (rowValue % 2 == 0)
                {
                    if (columnValue % 2 == 0)
                    {
                        boardPieces[rowValue].tablePiecePosition[columnValue].gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
                        boardPieces[rowValue].tablePiecePosition[columnValue].SetPlayable();
                        auxiliarBoardPiecesList.Add(boardPieces[rowValue].tablePiecePosition[columnValue]);
                    }
                }
                else
                {
                    if (columnValue % 2 != 0)
                    {
                        boardPieces[rowValue].tablePiecePosition[columnValue].gameObject.GetComponent<MeshRenderer>().material = blackMaterial;
                        boardPieces[rowValue].tablePiecePosition[columnValue].SetPlayable();
                        auxiliarBoardPiecesList.Add(boardPieces[rowValue].tablePiecePosition[columnValue]);
                    }
                }
                placeController--;
                if (columnValue < boardPieces[0].tablePiecePosition.Length)
                {
                    columnValue++;
                }

                if (placeController % boardPieces.Length == 0)
                {
                    rowValue++;
                    columnValue = 0;
                    playableArea.Add(auxiliarBoardPiecesList);
                    auxiliarBoardPiecesList = new List<BoardPiece>();
                }

            }
        }
        pc.PieceConstructor();
    }


    public List<List<BoardPiece>> GetPlaybleArea()
    {
        if (playableArea.Count > 0)     
        {
            return playableArea;
        }
        return null;
    }

    public Board GetBoard()
    {
        return board;
    }
}
