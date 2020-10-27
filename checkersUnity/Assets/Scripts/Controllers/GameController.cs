using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    Piece pieceToUpdate;
    int[] currentPos, newPos;

    private bool isPieceClicked = false;
    private GameObject clickedPiece = null;
    CurrentTable currentTable;
    bool isPiecePressed;
    bool isBlackTurn = false;
    int currentBoardPieceIndex;
    int[] eatPosition;
    List<List<int>> contactPosition;

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
        int row, column;
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
            bool isPieceInContactWihtOpponet = PieceInContact(currentPos[0], currentPos[1]);  

            GameObject newBoardPositionPiece = currentTable.GetCurretBoardPositions()[newPos[0]][newPos[1]].gameObject;

            if (((pieceToUpdate.GetIsUp() == true && (currentPos[0] - newPos[0]) == 1) || 
                (pieceToUpdate.GetIsUp() == false && (newPos[0] - currentPos[0]) == 1)) && 
                (currentPos[1] - newPos[1] == 1 ) || (currentPos[1] - newPos[1] == -1)) 
            {
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

                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    isBlackTurn = !isBlackTurn;
              
            }
            else 
            {
                Debug.Log("Cannot Move to there now");
            }
        }
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

    public bool GetBlackTurn()
    {
        return isBlackTurn;
    }
    private bool PieceInContact(int row, int column)
    {
        bool isPieceInContact = false;
        contactPosition = new List<List<int>>();

        if (pieceToUpdate.GetIsUp() == true)
        {

            if (currentPos[1] < currentTable.GetPiecesPosition()[column].Count - 1)
            {
                //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] - 1][currentPos[1] + 1] != null);
                //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] - 1][currentPos[1] + 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPos[0]][currentPos[1]].GetIsBlack());
                if (currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
                    currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    List<int> contactPositionValues = new List<int>();
                    contactPositionValues.Add(row - 1);
                    contactPositionValues.Add(column + 1);
                    contactPosition.Add(contactPositionValues);
                    isPieceInContact = true;

                }
            }
            if (column > 0)
            {
                //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] - 1][currentPos[1] + 1] != null);
                //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] - 1][currentPos[1] + 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPos[0]][currentPos[1]].GetIsBlack());
                if (currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
                    currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    List<int> contactPositionValues = new List<int>();
                    contactPositionValues.Add(row - 1);
                    contactPositionValues.Add(column - 1);
                    contactPosition.Add(contactPositionValues);
                    isPieceInContact = true;
                    
                }
            }
        }
        else
        {
            if (column < currentTable.GetPiecesPosition()[column].Count - 1)
            {
                if (currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
                    currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    List<int> contactPositionValues = new List<int>();
                    contactPositionValues.Add(row + 1);
                    contactPositionValues.Add(column + 1);
                    contactPosition.Add(contactPositionValues);
                    isPieceInContact = true;
                    //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] + 1][currentPos[1] + 1]);
                }
            }
            if (column > 0)
            {
                if (currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                    currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    List<int> contactPositionValues = new List<int>();
                    contactPositionValues.Add(row + 1);
                    contactPositionValues.Add(column - 1);
                    contactPosition.Add(contactPositionValues);
                    isPieceInContact = true;
                    //Debug.Log(currentTable.GetPiecesPosition()[currentPos[0] + 1][currentPos[1] - 1]);
                }
            }
        }
        return isPieceInContact;
    }
}
