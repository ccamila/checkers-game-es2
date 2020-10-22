using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    //List<List<BoardPiece>> playbleBoard;
    //List<List<Piece>> checkersPiecesPositions;
    //private bool turnController = false; // true black, false white 
    Piece pieceToUpdate;
    int[] currentPos, newPos;

    private bool isPieceClicked = false;
    private GameObject clickedPiece = null;
    CurrentTable currentTable;
    bool isPiecePressed;


    public static GameController instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindObjectOfType<GameController>();

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("GameController");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<GameController>();
                DontDestroyOnLoad(instanceObject);
            }
            else
                Debug.Log("Resource does not have a definition for PiecesContructor");
        }
        return _instance;
    }
    private void Awake()
    {
        tableConstructor = TableConstructor.instance();
        piecesConstructor = PiecesConstructor.instance();
        tableConstructor.ConstructBoard();
        piecesConstructor.ConstructPieces();
        currentTable = new CurrentTable(tableConstructor.GetBoard(), piecesConstructor.GetPiecesPosition());
        //Debug.Log(currentTable.GetCurrentBoard().gameObject.name);
        //playbleBoard = tableConstructor.GetPlaybleArea();
        //checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();
    }

    public void SetPiece(Piece piece)
    {
        pieceToUpdate = piece;
    }

    public void SetNewPOs(BoardPiece positionBoard)
    {
        bool checkObjectController = true;
        int indexOfList = 0;
        int row, 
            column;
        newPos = new int[2];


        while (checkObjectController)
        {

            if (currentTable.GetCurretBoardPositions()[indexOfList].Contains(positionBoard))
            {
                row = indexOfList;
                column = currentTable.GetCurretBoardPositions()[indexOfList].IndexOf(positionBoard);
                checkObjectController = false;
                newPos[0] = row;
                newPos[1] = column;
            }
            else if (indexOfList == currentTable.GetCurretBoardPositions().Count - 1)
            {
                checkObjectController = false;
            }
            else
            {

                indexOfList++;
            }
        }
    }

    public void SetOldPOs(int row, int column)
    {
        currentPos = new int[2];
        currentPos[0] = row;
        currentPos[1] = column;
    }
    public void updateGameobject()
    {


        if (pieceToUpdate)
        {
            GameObject newBoardPositionPiece = currentTable.GetCurretBoardPositions()[newPos[0]][newPos[1]].gameObject;
            Vector2 newPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

/*            Debug.Log(currentPos[0] + " corree peice " + currentPos[1]);*/
            currentTable.GetCurrentBoard().SetBoardPiecePlayable(currentPos[0], currentPos[1]);

            pieceToUpdate.gameObject.transform.position = newPosition;

            currentTable.UpdatePiecesPosition(newPos[0], newPos[1], pieceToUpdate);
/*            for (int i = 0; i < currentTable.GetCurrentBoard().GetBoardMatrix()[newPos[0]].Count; i++)
            {
                Debug.Log(currentTable.GetCurrentBoard().GetBoardMatrix()[newPos[0]][i].gameObject.name);


            }*/
            currentTable.UpdatePiecesPosition(currentPos[0], currentPos[1], null);
/*            for (int i = 0; i < currentTable.GetCurrentBoard().GetBoardMatrix()[newPos[0]].Count; i++)
            {
                Debug.Log(currentTable.GetCurrentBoard().GetBoardMatrix()[currentPos[0]][i].gameObject.name);

            }*/
        }
        pieceToUpdate = null;

        SetIsPieceClicked();
        SetClickedPiece(null);
    }

    public bool GetIsPieceClicked()
    {
        return isPieceClicked;
    }

    public void SetIsPieceClicked()
    {
        isPieceClicked = !isPieceClicked;
    }

    public GameObject GetClickedPiece()
    {
        return clickedPiece;
    }

    public void SetClickedPiece(GameObject currentClickedPiece)
    {
        clickedPiece = currentClickedPiece;
    }

    public CurrentTable GetCurrentTable()
    {
        return currentTable;
    }

    /*    private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();

    *//*            for (int i = 0; i < checkersPiecesPositions.Count; i++)
                {
                    for (int j = 0; j < checkersPiecesPositions[0].Count; j++)
                    {
                        Debug.Log(checkersPiecesPositions[i][j].gameObject.name + " " + i + " " + j);
                    }

                    //Debug.Log(checkersPiecesPositions[i].Contains(auxiliarPiece));
                }*//*
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    Piece auxiliarPiece = hit.transform.gameObject.GetComponent<Piece>();

                    //Debug.Log(System.Array.IndexOf(checkersPiecesPositions,auxiliarPiece));

                    //int row = checkersPiecesPositions.GetLength(0);
                    //int column
    *//*                for (int i = 0; i < checkersPiecesPositions.Count; i++)
                    {
                        for (int j = 0; j < checkersPiecesPositions[0].Count; j++)
                        {
                            Debug.Log(checkersPiecesPositions[i][j].gameObject.name + " " + i + " " + j);
                        }

                        //Debug.Log(checkersPiecesPositions[i].Contains(auxiliarPiece));
                    }*//*
                    //Debug.Log(checkersPiecesPositions.Find(hit.transform.gameObject.GetComponent<Piece>()));
                    //playbleBoard[0].Find(hit.transform.gameObject.GetComponent<BoardPiece>());
                }
            }
        }*/

}
