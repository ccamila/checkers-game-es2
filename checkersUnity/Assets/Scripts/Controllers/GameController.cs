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
    List<int> eatPositionLeft;
    List<int> eatPositionRight;
    List<int> contactPositionLeft;
    List<int> contacPositionRight;
    //List<List<int>> contactPositionleft;

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
    public void UpdateGameobject()
    {

        if (pieceToUpdate)
        {
            bool canEatLeft = false;
            bool canEatRight = false;
            bool isPieceInContactWihtOpponetLeft = PieceInContactLeft(currentPos[0], currentPos[1], false);
            if (isPieceInContactWihtOpponetLeft == true && contactPositionLeft[1] > 0)
            {
                canEatLeft = PieceInContactLeft(contactPositionLeft[0], contactPositionLeft[1], isPieceInContactWihtOpponetLeft);

            }
            bool isPieceInContactWihtOpponetRight = PieceInContactRight(currentPos[0], currentPos[1], false);
            if (isPieceInContactWihtOpponetRight)
            {
                canEatRight = PieceInContactRight(contacPositionRight[0], contacPositionRight[1], isPieceInContactWihtOpponetRight);
            }

            if (canEatLeft && (((newPos[0] != eatPositionLeft[0]) || newPos[1] != eatPositionLeft[1])) ||
                (canEatRight && ((newPos[0] != eatPositionRight[0]) || newPos[1] != eatPositionRight[1])))
            {
                Debug.Log("You have to eat now");
            }
            else if (canEatLeft && (newPos[0] == eatPositionLeft[0] && newPos[1] == eatPositionLeft[1]))
            {

                GameObject newBoardPositionPiece = currentTable.GetCurretBoardPositions()[newPos[0]][newPos[1]].gameObject;
                Vector2 newPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);


                currentTable.GetCurrentBoard().SetBoardPiecePlayable(currentPos[0], currentPos[1]);

                pieceToUpdate.gameObject.transform.position = newPosition;

                currentTable.UpdatePiecesPosition(newPos[0], newPos[1], pieceToUpdate);

                currentTable.UpdatePiecesPosition(currentPos[0], currentPos[1], null);

                currentTable.GetCurrentBoard().SetBoardPiecePlayable(contactPositionLeft[0], contactPositionLeft[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[contactPositionLeft[0]][contactPositionLeft[1]].gameObject;
                Destroy(pieceToDestroy);
                currentTable.UpdatePiecesPosition(contactPositionLeft[0], contactPositionLeft[1], null);
                pieceToUpdate = null;
                SetIsPieceClicked();
                SetClickedPiece(null);
                isBlackTurn = !isBlackTurn;

            }
            else if (canEatRight && (newPos[0] == eatPositionRight[0] && newPos[1] == eatPositionRight[1]))
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardPositions()[newPos[0]][newPos[1]].gameObject;
                Vector2 newPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);


                currentTable.GetCurrentBoard().SetBoardPiecePlayable(currentPos[0], currentPos[1]);

                pieceToUpdate.gameObject.transform.position = newPosition;

                currentTable.UpdatePiecesPosition(newPos[0], newPos[1], pieceToUpdate);

                currentTable.UpdatePiecesPosition(currentPos[0], currentPos[1], null);

                currentTable.GetCurrentBoard().SetBoardPiecePlayable(contacPositionRight[0], contacPositionRight[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[contacPositionRight[0]][contacPositionRight[1]].gameObject;
                Destroy(pieceToDestroy);
                currentTable.UpdatePiecesPosition(contacPositionRight[0], contacPositionRight[1], null);
                pieceToUpdate = null;
                SetIsPieceClicked();
                SetClickedPiece(null);
                isBlackTurn = !isBlackTurn;
            }
            else
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardPositions()[newPos[0]][newPos[1]].gameObject;

                if (((pieceToUpdate.GetIsUp() == true && (currentPos[0] - newPos[0]) == 1) ||
                    (pieceToUpdate.GetIsUp() == false && (newPos[0] - currentPos[0]) == 1)) &&
                    (currentPos[1] - newPos[1] == 1) || (currentPos[1] - newPos[1] == -1))
                {
                    Vector2 newPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                    currentTable.GetCurrentBoard().SetBoardPiecePlayable(currentPos[0], currentPos[1]);

                    pieceToUpdate.gameObject.transform.position = newPosition;

                    currentTable.UpdatePiecesPosition(newPos[0], newPos[1], pieceToUpdate);

                    currentTable.UpdatePiecesPosition(currentPos[0], currentPos[1], null);

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
    private bool PieceInContactLeft(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (pieceToUpdate.GetIsUp() == true)
        {
            if (column > 0)
            {
                if (hasContatc == false)
                {
                    if (currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
                        currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                    {
                        contactPositionLeft = new List<int>();
                        contactPositionLeft.Add(row - 1);
                        contactPositionLeft.Add(column - 1);
                        isPieceInContact = true;
                    }
                }
                else
                {
                    if (currentTable.GetPiecesPosition()[row - 1][column - 1] == null)
                    {
                        eatPositionLeft = new List<int>();
                        eatPositionLeft.Add(row - 1);
                        eatPositionLeft.Add(column - 1);
                        canEat = true;
                    }
                }
            }
        }
        else
        {
            if (column > 0)
            {
                if (hasContatc == false)
                {
                    if (currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                    currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                    {
                        contactPositionLeft = new List<int>();
                        contactPositionLeft.Add(row + 1);
                        contactPositionLeft.Add(column - 1);
                        isPieceInContact = true;
                    }
                }
                else
                {
                    if (currentTable.GetPiecesPosition()[row + 1][column - 1] == null)
                    {
                        eatPositionLeft = new List<int>();
                        eatPositionLeft.Add(row + 1);
                        eatPositionLeft.Add(column - 1);
                        canEat = true;
                    }
                }
            }
        }
        if (hasContatc == false)
        {
            finalCheck = isPieceInContact;
        }
        else
        {
            finalCheck = canEat;
        }
        return finalCheck;
    }
    private bool PieceInContactRight(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;


        if (pieceToUpdate.GetIsUp() == true)
        {

            if (currentPos[1] < currentTable.GetPiecesPosition()[column].Count - 1)
            {
                if (hasContatc == false)
                {
                    if (currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
                       currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                    {
                        contacPositionRight = new List<int>();
                        contacPositionRight.Add(row - 1);
                        contacPositionRight.Add(column + 1);
                        isPieceInContact = true;
                    }
                }
                else
                {
                    if (currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
                    {
                        eatPositionRight = new List<int>();
                        eatPositionRight.Add(row - 1);
                        eatPositionRight.Add(column + 1);
                        canEat = true;
                    }
                }
            }
        }
        else
        {
            if (column < currentTable.GetPiecesPosition()[column].Count - 1)
            {
                if (hasContatc == false)
                {
                    if (currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
                        currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                    {
                        contacPositionRight = new List<int>();
                        contacPositionRight.Add(row + 1);
                        contacPositionRight.Add(column + 1);
                        isPieceInContact = true;
                    }
                }
                else
                {
                    if (currentTable.GetPiecesPosition()[row + 1][column + 1] == null)
                    {
                        eatPositionRight = new List<int>();
                        eatPositionRight.Add(row + 1);
                        eatPositionRight.Add(column + 1);
                        canEat = true;
                    }
                }
            }
        }
        if (hasContatc == false)
        {
            finalCheck = isPieceInContact;
        }
        else
        {
            finalCheck = canEat;
        }
        return finalCheck;
    }
}
