using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableConstructor : MonoBehaviour
{
    [SerializeField]
    private Transform centralPosition;
    [SerializeField]
    private Material blackMaterial;
    GameObject boardGameObject;
    private Board board;
    private int totalPieces;

    private List<List<BoardPiece>> playableArea;


    private void Awake()
    {
        playableArea = new List<List<BoardPiece>>();
        BoardCounstructor();

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    }

    public List<List<BoardPiece>> GetPlaybleArea()
    {
        if (playableArea.Count > 0)     
        {
            return playableArea;
        }
        return null;
    }
}
