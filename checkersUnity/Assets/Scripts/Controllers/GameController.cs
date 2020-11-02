using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    IAPlayerController iaPlayerController;
    Piece pieceToUpdate;
    int[] currentPosition, newPosition;

    private bool isPieceClicked = false;
    private bool mandatoryEat = false;
    private GameObject clickedPiece = null;
    CurrentTable currentTable;
    bool isBlackTurn = false;
    bool hasEaten = false;
    List<int> eatPositionLeft;
    List<int> eatPositionRight;
    List<int> contactPositionLeft;
    List<int> contacPositionRight;
    List<int> positionUpLeft;
    List<int> positionUpRight;
    List<int> positionDownLeft;
    List<int> positionDownRight;
    List<List<int>> placesToEatAgain;

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
        iaPlayerController = IAPlayerController.Instance();
        tableConstructor.ConstructBoard();
        piecesConstructor.ConstructPieces();
        iaPlayerController.SetGameController();
        currentTable = new CurrentTable(tableConstructor.GetBoard(), piecesConstructor.GetPiecesPosition());
        //Debug.Log(currentTable.GetCurrentBoard().gameObject.name);
        //playbleBoard = tableConstructor.GetPlaybleArea();
        //checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();
    }

    public void SetPiece(Piece piece)
    {
        pieceToUpdate = piece;
    }

    public void SetNewBoardPosition(BoardPiece positionBoard)
    {
        bool checkObjectController = true;
        int indexOfList = 0;
        int row, column;
        newPosition = new int[2];


        while (checkObjectController)
        {

            if (currentTable.GetCurretBoardSpacePositions()[indexOfList].Contains(positionBoard))
            {
                row = indexOfList;
                column = currentTable.GetCurretBoardSpacePositions()[indexOfList].IndexOf(positionBoard);
                checkObjectController = false;
                newPosition[0] = row;
                newPosition[1] = column;
            }
            else if (indexOfList == currentTable.GetCurretBoardSpacePositions().Count - 1)
            {
                checkObjectController = false;
            }
            else
            {

                indexOfList++;
            }
        }
    }

    public void SetOldPosition(int row, int column)
    {
        currentPosition = new int[2];
        currentPosition[0] = row;
        currentPosition[1] = column;
    }
   
    private void UpdateTurn(bool setMandatoryEatFalse)
    {
        pieceToUpdate = null;
        SetIsPieceClicked();
        SetClickedPiece(null);
        if (setMandatoryEatFalse)
        {
            mandatoryEat = false;
        }
        isBlackTurn = !isBlackTurn;
        if( (isBlackTurn && iaPlayerController.GetIsIAPlayerBlack()) || (!isBlackTurn && !iaPlayerController.GetIsIAPlayerBlack()))
        {
            if (!iaPlayerController.MakeAMove())
            {
                Debug.Log("AI cant move");
            }

        }
           
    }
    public void UpdateGameObject()
    {
        
        if (pieceToUpdate)
        {

            bool canEatLeft = false;
            bool canEatRight = false;
            bool isPieceInContactWihtOpponetLeft = PieceInContactLeft(currentPosition[0], currentPosition[1], false);
            if (isPieceInContactWihtOpponetLeft == true && contactPositionLeft[1] > 0)
            {
                canEatLeft = PieceInContactLeft(contactPositionLeft[0], contactPositionLeft[1], isPieceInContactWihtOpponetLeft);

            }
            bool isPieceInContactWihtOpponetRight = PieceInContactRight(currentPosition[0], currentPosition[1], false);
            if (isPieceInContactWihtOpponetRight)
            {
                canEatRight = PieceInContactRight(contacPositionRight[0], contacPositionRight[1], isPieceInContactWihtOpponetRight);
            }

            if (canEatLeft && (((newPosition[0] != eatPositionLeft[0]) || newPosition[1] != eatPositionLeft[1])) ||
                (canEatRight && ((newPosition[0] != eatPositionRight[0]) || newPosition[1] != eatPositionRight[1])))
            {
                //Debug.Log("You have to eat now");
            }
            else if (canEatLeft && (newPosition[0] == eatPositionLeft[0] && newPosition[1] == eatPositionLeft[1]))
            {

                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                pieceToUpdate.gameObject.transform.position = newBoardPosition;

                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentPosition[0] = newPosition[0];
                currentPosition[1] = newPosition[1];
                currentTable. GetCurrentBoard().SetBoardSpacePlayable(contactPositionLeft[0], contactPositionLeft[1]);

                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[contactPositionLeft[0]][contactPositionLeft[1]].gameObject;
                Destroy(pieceToDestroy);
                currentTable.UpdatePiecesPosition(contactPositionLeft[0], contactPositionLeft[1], null);

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                if (checkToEatAgain)
                {
                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                    /*pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }

            }
            else if (canEatRight && (newPosition[0] == eatPositionRight[0] && newPosition[1] == eatPositionRight[1]))
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                pieceToUpdate.gameObject.transform.position = newBoardPosition;

                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentPosition[0] = newPosition[0];
                currentPosition[1] = newPosition[1];

                currentTable.GetCurrentBoard().SetBoardSpacePlayable(contacPositionRight[0], contacPositionRight[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[contacPositionRight[0]][contacPositionRight[1]].gameObject;
                Debug.Log(pieceToDestroy);
                Destroy(pieceToDestroy);
                currentTable.UpdatePiecesPosition(contacPositionRight[0], contacPositionRight[1], null);

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                if (checkToEatAgain)
                {
                   
                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                   /* pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }
                //hasEaten = true;
            }
            else
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;

                if (((pieceToUpdate.GetIsUp() == true && (currentPosition[0] - newPosition[0]) == 1) ||
                    (pieceToUpdate.GetIsUp() == false && (newPosition[0] - currentPosition[0]) == 1)) &&
                    (currentPosition[1] - newPosition[1] == 1) || (currentPosition[1] - newPosition[1] == -1))
                {
                    Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                    currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                    pieceToUpdate.gameObject.transform.position = newBoardPosition;

                    currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                    currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                    currentPosition[0] = newPosition[0];
                    currentPosition[1] = newPosition[1];

                    this.UpdateTurn(false);
                    /*pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    isBlackTurn = !isBlackTurn;*/

                }
                else
                {
                    Debug.Log("Cannot Move to there now");
                }
                //}
            }
        }
    }

    public void UpdateGameObjectBlockedDueMandatoryEat()
    {
/*        Debug.Log(positionUpLeft);
        Debug.Log(positionDownLeft);
        Debug.Log(positionDownRight);
        Debug.Log(positionUpRight);*/
        if (positionUpLeft != null)
        {
            if (newPosition[0] == positionUpLeft[0] && newPosition[1] == positionUpLeft[1])
            {
                Debug.Log("1111111111");
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[positionUpLeft[2]][positionUpLeft[3]].gameObject;
                Destroy(pieceToDestroy);
                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionUpLeft[2], positionUpLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionUpRight[2], positionUpRight[3]);
                /*currentTable.GetCurretBoardSpacePositions()[positionUpLeft[2]][positionUpLeft[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                   /* pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }


            }
            else
            {
                Debug.Log("You have to eat at up left");
            }
        }
         if (positionUpRight != null)
        {
            if (newPosition[0] == positionUpRight[0] && newPosition[1] == positionUpRight[1])
            {
                Debug.Log("222222222222222222222");
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[positionUpRight[2]][positionUpRight[3]].gameObject;
                Destroy(pieceToDestroy);
                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionUpRight[2], positionUpRight[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionUpRight[2], positionUpRight[3]);
                /*currentTable.GetCurretBoardSpacePositions()[positionUpRight[2]][positionUpRight[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {
                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                    /*pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }

            }
            else
            {
                Debug.Log("You have to eat at up right");
            }
        }
        if (positionDownLeft != null)
        {
            if (newPosition[0] == positionDownLeft[0] && newPosition[1] == positionDownLeft[1])
            {
                Debug.Log("333333333333333333");
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[positionDownLeft[2]][positionDownLeft[3]].gameObject;
                Destroy(pieceToDestroy);
                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownLeft[2], positionDownLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionUpRight[2], positionUpRight[3]);
               /* currentTable.GetCurretBoardSpacePositions()[positionDownLeft[2]][positionDownLeft[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                   /* pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }

            }
            else
            {
                Debug.Log("You have to eat at down left");
            }
        }
        if (positionDownRight != null)
        {
            if (newPosition[0] == positionDownRight[0] && newPosition[1] == positionDownRight[1])
            {
                Debug.Log("44444444444444444");
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                GameObject pieceToDestroy = currentTable.GetPiecesPosition()[positionDownRight[2]][positionDownRight[3]].gameObject;
                Destroy(pieceToDestroy);

                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownRight[2], positionDownRight[3], null);
                Debug.Log(positionUpRight[2]);
                Debug.Log(positionUpRight[3]);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionUpRight[2], positionUpRight[3]);
                /*currentTable.GetCurretBoardSpacePositions()[positionDownRight[2]][positionDownRight[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    this.UpdateTurn(true);
                   /* pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;*/
                }

            }
            else 
            {
                Debug.Log("You have to eat at down right");
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
    public bool GetMandatoryEat()
    {
        return mandatoryEat;
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
                    if (row - 1 >= 0 && column -1 >= 0 &&  currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
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
                    if (row - 1 >= 0 && column - 1 >= 0 &&  currentTable.GetPiecesPosition()[row - 1][column - 1] == null)
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
                    if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
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
                    if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] == null)
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

            if (currentPosition[1] < currentTable.GetPiecesPosition()[column].Count - 1)
            {
                if (hasContatc == false)
                {
                    if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row - 1 >= 0 && 
                        currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
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
                    if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row -1 >= 0 && 
                        currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
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
                    if (row + 1 < currentTable.GetPiecesPosition()[0].Count && 
                        column + 1 < currentTable.GetPiecesPosition()[0].Count &&
                        currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
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
                    if (row + 1 < currentTable.GetPiecesPosition()[0].Count &&
                        column + 1 < currentTable.GetPiecesPosition()[0].Count && 
                        currentTable.GetPiecesPosition()[row + 1][column + 1] == null)
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

    private bool CheckPieceDiagonals(int row, int column)
    {
        bool checkIfHasToEat = false;

        if (row < currentTable.GetPiecesPosition().Count - 1 && column < currentTable.GetPiecesPosition().Count - 1)
        {
            if (currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
                currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
            {
                if (row < currentTable.GetPiecesPosition().Count - 2 && column < currentTable.GetPiecesPosition().Count - 2)
                {
                    if (currentTable.GetPiecesPosition()[row + 2][column + 2] == null)

                    {
                        Debug.Log("ok ok ok ");
                        positionUpRight = new List<int>();
                        Debug.Log("ok ok ok 4");
                        positionUpRight.Add(row + 2);
                        Debug.Log("ok ok ok 45");
                        positionUpRight.Add(column + 2);
                        Debug.Log("ok ok ok 6");
                        positionUpRight.Add(row + 1);
                        Debug.Log("ok ok ok 7");
                        positionUpRight.Add(column + 1);
                        Debug.Log("ok ok ok 8");
                        Debug.Log("able to eat again at +2+2");
                        checkIfHasToEat = true;
                    }
                }

            }
        }
        if (row < currentTable.GetPiecesPosition().Count - 1 && column > 0)
        {
            if (currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
            {
                if (row < currentTable.GetPiecesPosition().Count - 2 && column - 1 > 0)
                {
                    if (currentTable.GetPiecesPosition()[row + 2][column - 2] == null)
                    {
                        positionUpLeft = new List<int>();
                        positionUpLeft.Add(row + 2);
                        positionUpLeft.Add(column - 2);
                        positionUpLeft.Add(row + 1);
                        positionUpLeft.Add(column - 1);
                        Debug.Log("able to eat again at +2-2");
                        checkIfHasToEat = true;
                    }
                }
            }
        }
        if (row > 0 && column < currentTable.GetPiecesPosition().Count - 1)
        {
            if (currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
                currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
            {
                if (row - 1 > 0 && column < currentTable.GetPiecesPosition().Count - 2)
                {
                    if (currentTable.GetPiecesPosition()[row - 2][column + 2] == null)
                    {
                        positionDownRight = new List<int>();
                        positionDownRight.Add(row - 2);
                        positionDownRight.Add(column + 2);
                        positionDownRight.Add(row - 1);
                        positionDownRight.Add(column + 1);
                        Debug.Log("able to check for eat again at -2+2");
                        checkIfHasToEat = true;
                    }
                }
            }
        }
        if (row > 0 && column > 0)
        {
            if (currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
                currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
            {
                if (row - 1 > 0 && column - 1 > 0)
                {
                    if (currentTable.GetPiecesPosition()[row - 2][column - 2] == null)
                    {
                        positionDownLeft = new List<int>();
                        positionDownLeft.Add(row - 2);
                        positionDownLeft.Add(column - 2);
                        positionDownLeft.Add(row - 1);
                        positionDownLeft.Add(column - 1);
                        Debug.Log("able to eat again at -2-2");
                        checkIfHasToEat = true;
                    }
                }
            }
        }
        return checkIfHasToEat;
    }
}
