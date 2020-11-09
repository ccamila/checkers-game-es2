using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    Piece pieceToUpdate;
    int[] currentPosition, newPosition;

    private bool isPieceClicked = false;
    private bool mandatoryEat = false;
    private GameObject clickedPiece = null;
    CurrentTable currentTable;
    bool isBlackTurn = false;
    bool hasEaten = false;
    List<int> eatPositionUpLeft;
    List<int> eatPositionUpRight;
    List<int> eatPositionDownLeft;
    List<int> eatPositionDownRight;
    List<int> contactPositionUpLeft;
    List<int> contacPositionUpRight;
    List<int> contactPositionDownLeft;
    List<int> contacPositionDownRight;
    List<int> positionUpLeft;
    List<int> positionUpRight;
    List<int> positionDownLeft;
    List<int> positionDownRight;

    Dictionary<int, List<int>> positionToEatAgain;
    int dictionaryIndexController = 0;
    List<Piece> piecesToEat;
    List<List<int>> piecesToEatPositions;
    bool initialTurnEatLock = false;
    List<Piece> piecesThatHasToEatInBigginingOfTurn;

    [SerializeField]
    private bool obrigatoryEat = true;

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

    public void SetCurrentClickedPiece(Piece piece)
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

    public void SetOldPieceClickedPosition(int row, int column)
    {
        currentPosition = new int[2];
        currentPosition[0] = row;
        currentPosition[1] = column;
    }
    public void UpdateGameObject()
    {
        if (initialTurnEatLock == false)
        {
            if (pieceToUpdate && piecesThatHasToEatInBigginingOfTurn.Count > 0 && piecesThatHasToEatInBigginingOfTurn.Contains(pieceToUpdate))
            {
                piecesToEat = new List<Piece>();
                List<int> auxiliarPositionList = new List<int>();
                piecesToEatPositions = new List<List<int>>();
                bool canEatUpLeft = false;
                bool canEatUpRight = false;
                bool canEatDownLeft = false;
                bool canEatDownRight = false;
                bool isPieceInContactWihtOpponentUpLeft = PieceInContactUpLeft(currentPosition[0], currentPosition[1], false);
                if (isPieceInContactWihtOpponentUpLeft == true && contactPositionUpLeft[1] > 0)
                {
                    canEatUpLeft = PieceInContactUpLeft(contactPositionUpLeft[0], contactPositionUpLeft[1], isPieceInContactWihtOpponentUpLeft);

                }
                bool isPieceInContactWihtOpponentUpRight = PieceInContactUpRight(currentPosition[0], currentPosition[1], false);
                if (isPieceInContactWihtOpponentUpRight)
                {
                    canEatUpRight = PieceInContactUpRight(contacPositionUpRight[0], contacPositionUpRight[1], isPieceInContactWihtOpponentUpRight);
                }
                bool isPieceInContactWihtOpponentDownLeft = PieceInContactDownLeft(currentPosition[0], currentPosition[1], false);
                if (isPieceInContactWihtOpponentDownLeft == true && contactPositionDownLeft[1] > 0)
                {
                    canEatDownLeft = PieceInContactDownLeft(contactPositionDownLeft[0], contactPositionDownLeft[1], isPieceInContactWihtOpponentDownLeft);

                }
                bool isPieceInContactWihtOpponentDownRight = PieceInContactDownRight(currentPosition[0], currentPosition[1], false);
                if (isPieceInContactWihtOpponentDownRight)
                {
                    canEatDownRight = PieceInContactDownRight(contacPositionDownRight[0], contacPositionDownRight[1], isPieceInContactWihtOpponentDownRight);
                }

                if (canEatUpLeft && (newPosition[0] == eatPositionUpLeft[0] && newPosition[1] == eatPositionUpLeft[1]))
                {
                    if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]]))
                    {
                        GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                        pieceToUpdate.gameObject.transform.position = newBoardPosition;

                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];
                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(contactPositionUpLeft[0], contactPositionUpLeft[1]);
                        List<int> auxiliarPositiontoEatList = new List<int>();
                        auxiliarPositiontoEatList.Add(contactPositionUpLeft[0]);
                        auxiliarPositiontoEatList.Add(contactPositionUpLeft[1]);
                        piecesToEat.Add(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]]);
                        if (positionToEatAgain == null)
                        {
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                        positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                        dictionaryIndexController++;

                        currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].SetIsAvaiableToEat(false);

                        bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                        Debug.Log(checkToEatAgain + " EATT again UP LEF");
                        if (checkToEatAgain)
                        {
                            mandatoryEat = true;
                        }
                        else
                        {
                            pieceToUpdate = null;
                            SetIsPieceClicked();
                            SetClickedPiece(null);
                            mandatoryEat = false;
                            isBlackTurn = !isBlackTurn;
                            piecesThatHasToEatInBigginingOfTurn = null;
                            piecesToEat = new List<Piece>();
                            Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject.name);
                            Destroy(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject);
                            currentTable.UpdatePiecesPosition(contactPositionUpLeft[0], contactPositionUpLeft[1], null);
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("jogada morre aqui, liberar ");
                    }

                }
                else if (canEatUpRight && (newPosition[0] == eatPositionUpRight[0] && newPosition[1] == eatPositionUpRight[1]))
                {
                    if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contacPositionUpRight[0]][contacPositionUpRight[1]]))
                    {
                        GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                        pieceToUpdate.gameObject.transform.position = newBoardPosition;

                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(contacPositionUpRight[0], contacPositionUpRight[1]);

                        List<int> auxiliarPositiontoEatList = new List<int>();
                        auxiliarPositiontoEatList.Add(contacPositionUpRight[0]);
                        auxiliarPositiontoEatList.Add(contacPositionUpRight[1]);
                        piecesToEat.Add(currentTable.GetPiecesPosition()[contacPositionUpRight[0]][contacPositionUpRight[1]]);
                        if (positionToEatAgain == null)
                        {
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                        positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                        dictionaryIndexController++;

                        currentTable.GetPiecesPosition()[contacPositionUpRight[0]][contacPositionUpRight[1]].SetIsAvaiableToEat(false);

                        bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                        Debug.Log(checkToEatAgain + " EATT again UP RIG");
                        if (checkToEatAgain)
                        {
                            mandatoryEat = true;
                        }
                        else
                        {
                            pieceToUpdate = null;
                            SetIsPieceClicked();
                            SetClickedPiece(null);
                            mandatoryEat = false;
                            isBlackTurn = !isBlackTurn;
                            piecesThatHasToEatInBigginingOfTurn = null;
                            Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contacPositionUpRight[0]][contacPositionUpRight[1]].gameObject.name);
                            Destroy(currentTable.GetPiecesPosition()[contacPositionUpRight[0]][contacPositionUpRight[1]].gameObject);
                            currentTable.UpdatePiecesPosition(contacPositionUpRight[0], contacPositionUpRight[1], null);
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;

                        }
                    }
                    else
                    {
                        Debug.Log("jogada morre aqui, liberar ");
                    }

                    //hasEaten = true;
                }
                else if (canEatDownLeft && (newPosition[0] == eatPositionDownLeft[0] && newPosition[1] == eatPositionDownLeft[1]))
                {
                    if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]]))
                    {

                        GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                        pieceToUpdate.gameObject.transform.position = newBoardPosition;

                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];
                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(contactPositionDownLeft[0], contactPositionDownLeft[1]);

                        List<int> auxiliarPositiontoEatList = new List<int>();
                        auxiliarPositiontoEatList.Add(contactPositionDownLeft[0]);
                        auxiliarPositiontoEatList.Add(contactPositionDownLeft[1]);
                        piecesToEat.Add(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]]);
                        if (positionToEatAgain == null)
                        {
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                        positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                        dictionaryIndexController++;

                        currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].SetIsAvaiableToEat(false);

                        bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                        Debug.Log(checkToEatAgain + " EATT again DWn LEF");
                        if (checkToEatAgain)
                        {
                            mandatoryEat = true;
                        }
                        else
                        {
                            pieceToUpdate = null;
                            SetIsPieceClicked();
                            SetClickedPiece(null);
                            mandatoryEat = false;
                            isBlackTurn = !isBlackTurn;
                            piecesThatHasToEatInBigginingOfTurn = null;
                            Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject.name);
                            Destroy(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject);
                            currentTable.UpdatePiecesPosition(contactPositionDownLeft[0], contactPositionDownLeft[1], null);
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("jogada morre aqui, liberar ");
                    }

                }
                else if (canEatDownRight && (newPosition[0] == eatPositionDownRight[0] && newPosition[1] == eatPositionDownRight[1]))
                {
                    if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]]))
                    {

                        GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                        pieceToUpdate.gameObject.transform.position = newBoardPosition;

                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];

                        currentTable.GetCurrentBoard().SetBoardSpacePlayable(contacPositionDownRight[0], contacPositionDownRight[1]);

                        List<int> auxiliarPositiontoEatList = new List<int>();
                        auxiliarPositiontoEatList.Add(contacPositionDownRight[0]);
                        auxiliarPositiontoEatList.Add(contacPositionDownRight[1]);
                        piecesToEat.Add(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]]);
                        if (positionToEatAgain == null)
                        {
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                        positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                        dictionaryIndexController++;

                        bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                        currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].SetIsAvaiableToEat(false);

                        Debug.Log(checkToEatAgain + " EATT again DWN RIG");
                        if (checkToEatAgain)
                        {

                            mandatoryEat = true;
                        }
                        else
                        {
                            pieceToUpdate = null;
                            SetIsPieceClicked();
                            SetClickedPiece(null);
                            mandatoryEat = false;
                            isBlackTurn = !isBlackTurn;
                            piecesThatHasToEatInBigginingOfTurn = null;
                            Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject.name);
                            Destroy(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject);
                            currentTable.UpdatePiecesPosition(contacPositionDownRight[0], contacPositionDownRight[1], null);
                            positionToEatAgain = new Dictionary<int, List<int>>();
                            dictionaryIndexController = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("jogada morre aqui, liberar ");
                    }
                    //hasEaten = true;
                }
            }
            else if (pieceToUpdate && piecesThatHasToEatInBigginingOfTurn.Count == 0)
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;

                /*                    Debug.Log(pieceToUpdate.GetIsUp() == true);
                                    Debug.Log(currentPosition[1] - newPosition[1] + " rig");
                                    Debug.Log(currentPosition[1] - newPosition[1] + " lef");*/
                if (((pieceToUpdate.GetIsUp() == true && (currentPosition[0] - newPosition[0]) == 1) ||
                    (pieceToUpdate.GetIsUp() == false && (newPosition[0] - currentPosition[0]) == 1)) &&
                    (currentPosition[1] - newPosition[1] == 1) || (currentPosition[1] - newPosition[1] == -1))
                {
                    Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                    pieceToUpdate.gameObject.transform.position = newBoardPosition;
                    currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                    currentTable.GetCurrentBoard().SetBoardSpacePlayable(newPosition[0], newPosition[1]);
                    currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                    currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                    currentPosition[0] = newPosition[0];
                    currentPosition[1] = newPosition[1];
                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    isBlackTurn = !isBlackTurn;
                    piecesThatHasToEatInBigginingOfTurn = null;

                }
                else
                {
                    Debug.Log("Cannot Move to there now");
                }
            }
        }
    }

    public void UpdateGameObjectBlockedDueMandatoryEat()
    {
        if (positionUpLeft != null)
        {
            if (newPosition[0] == positionUpLeft[0] && newPosition[1] == positionUpLeft[1])
            {
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);

                List<int> auxiliarPositiontoEatList = new List<int>();
                auxiliarPositiontoEatList.Add(positionUpLeft[0]);
                auxiliarPositiontoEatList.Add(positionUpLeft[1]);
                piecesToEat.Add(currentTable.GetPiecesPosition()[positionUpLeft[2]][positionUpLeft[3]]);
                if (positionToEatAgain == null)
                {
                    positionToEatAgain = new Dictionary<int, List<int>>();
                    dictionaryIndexController = 0;
                }
                positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                dictionaryIndexController++;


                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionUpLeft[2], positionUpLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionUpLeft[2], positionUpLeft[3]);

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;
                    piecesThatHasToEatInBigginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardSpacePlayable(boardPieceList[0], boardPieceList[1]);

                        }
                        positionToEatAgain = new Dictionary<int, List<int>>();
                        dictionaryIndexController = 0;
                    }
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
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                List<int> auxiliarPositiontoEatList = new List<int>();
                auxiliarPositiontoEatList.Add(positionUpRight[0]);
                auxiliarPositiontoEatList.Add(positionUpRight[1]);
                piecesToEat.Add(currentTable.GetPiecesPosition()[positionUpRight[2]][positionUpRight[3]]);
                if (positionToEatAgain == null)
                {
                    positionToEatAgain = new Dictionary<int, List<int>>();
                    dictionaryIndexController = 0;
                }
                positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                dictionaryIndexController++;

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
                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;
                    piecesThatHasToEatInBigginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardSpacePlayable(boardPieceList[0], boardPieceList[1]);

                        }
                        positionToEatAgain = new Dictionary<int, List<int>>();
                        dictionaryIndexController = 0;
                    }
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
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                List<int> auxiliarPositiontoEatList = new List<int>();
                auxiliarPositiontoEatList.Add(positionDownLeft[0]);
                auxiliarPositiontoEatList.Add(positionDownLeft[1]);
                piecesToEat.Add(currentTable.GetPiecesPosition()[positionDownLeft[2]][positionDownLeft[3]]);
                if (positionToEatAgain == null)
                {
                    positionToEatAgain = new Dictionary<int, List<int>>();
                    dictionaryIndexController = 0;
                }
                positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                dictionaryIndexController++;

                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownLeft[2], positionDownLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionDownLeft[2], positionDownLeft[3]);
                /* currentTable.GetCurretBoardSpacePositions()[positionDownLeft[2]][positionDownLeft[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;
                    piecesThatHasToEatInBigginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);

                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardSpacePlayable(boardPieceList[0], boardPieceList[1]);
                        }
                        positionToEatAgain = new Dictionary<int, List<int>>();
                        dictionaryIndexController = 0;
                    }

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
                GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                currentTable.GetCurrentBoard().SetBoardSpacePlayable(currentPosition[0], currentPosition[1]);
                List<int> auxiliarPositiontoEatList = new List<int>();
                auxiliarPositiontoEatList.Add(positionDownRight[0]);
                auxiliarPositiontoEatList.Add(positionDownRight[1]);
                piecesToEat.Add(currentTable.GetPiecesPosition()[positionDownRight[2]][positionDownRight[3]]);
                if (positionToEatAgain == null)
                {
                    positionToEatAgain = new Dictionary<int, List<int>>();
                    dictionaryIndexController = 0;
                }
                positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                dictionaryIndexController++;

                pieceToUpdate.gameObject.transform.position = newBoardPosition;
                currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownRight[2], positionDownRight[3], null);
                currentTable.GetCurrentBoard().SetBoardSpacePlayable(positionDownRight[2], positionDownRight[3]);
                /*currentTable.GetCurretBoardSpacePositions()[positionDownRight[2]][positionDownRight[3]].SetPlayable();*/

                bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                if (checkToEatAgain)
                {

                    mandatoryEat = true;
                }
                else
                {
                    pieceToUpdate = null;
                    SetIsPieceClicked();
                    SetClickedPiece(null);
                    mandatoryEat = false;
                    isBlackTurn = !isBlackTurn;
                    piecesThatHasToEatInBigginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardSpacePlayable(boardPieceList[0], boardPieceList[1]);

                        }
                        positionToEatAgain = new Dictionary<int, List<int>>();
                        dictionaryIndexController = 0;
                    }
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
    private bool PieceInContactUpLeft(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;


        if (column > 0)
        {
            if (hasContatc == false)
            {
                if (row - 1 >= 0 && column - 1 >= 0 && currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
                    currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    contactPositionUpLeft = new List<int>();
                    contactPositionUpLeft.Add(row - 1);
                    contactPositionUpLeft.Add(column - 1);
                    isPieceInContact = true;
                }
            }
            else
            {
                if (row - 1 >= 0 && column - 1 >= 0 && currentTable.GetPiecesPosition()[row - 1][column - 1] == null)
                {
                    eatPositionUpLeft = new List<int>();
                    eatPositionUpLeft.Add(row - 1);
                    eatPositionUpLeft.Add(column - 1);
                    canEat = true;
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

    private bool PieceInContactUpRight(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (currentPosition[1] < currentTable.GetPiecesPosition()[column].Count - 1)
        {
            if (hasContatc == false)
            {
                if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row - 1 >= 0 &&
                    currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
                   currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    contacPositionUpRight = new List<int>();
                    contacPositionUpRight.Add(row - 1);
                    contacPositionUpRight.Add(column + 1);
                    isPieceInContact = true;
                }
            }
            else
            {
                if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row - 1 >= 0 &&
                    currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
                {
                    eatPositionUpRight = new List<int>();
                    eatPositionUpRight.Add(row - 1);
                    eatPositionUpRight.Add(column + 1);
                    canEat = true;
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

    private bool PieceInContactDownLeft(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;


        if (column > 0)
        {
            if (hasContatc == false)
            {
                if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    contactPositionDownLeft = new List<int>();
                    contactPositionDownLeft.Add(row + 1);
                    contactPositionDownLeft.Add(column - 1);
                    isPieceInContact = true;
                }
            }
            else
            {
                if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] == null)
                {
                    eatPositionDownLeft = new List<int>();
                    eatPositionDownLeft.Add(row + 1);
                    eatPositionDownLeft.Add(column - 1);
                    canEat = true;
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

    private bool PieceInContactDownRight(int row, int column, bool hasContatc)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (column < currentTable.GetPiecesPosition()[column].Count - 1)
        {
            if (hasContatc == false)
            {
                if (row + 1 < currentTable.GetPiecesPosition()[0].Count &&
                    column + 1 < currentTable.GetPiecesPosition()[0].Count &&
                    currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
                    currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                {
                    contacPositionDownRight = new List<int>();
                    contacPositionDownRight.Add(row + 1);
                    contacPositionDownRight.Add(column + 1);
                    isPieceInContact = true;
                }
            }
            else
            {
                if (row + 1 < currentTable.GetPiecesPosition()[0].Count &&
                    column + 1 < currentTable.GetPiecesPosition()[0].Count &&
                    currentTable.GetPiecesPosition()[row + 1][column + 1] == null)
                {
                    eatPositionDownRight = new List<int>();
                    eatPositionDownRight.Add(row + 1);
                    eatPositionDownRight.Add(column + 1);
                    canEat = true;
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
                currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack() &&
                !piecesToEat.Contains(currentTable.GetPiecesPosition()[row + 1][column + 1]))
            {
                if (row < currentTable.GetPiecesPosition().Count - 2 && column < currentTable.GetPiecesPosition().Count - 2)
                {
                    if (currentTable.GetPiecesPosition()[row + 2][column + 2] == null)

                    {
                        positionUpRight = new List<int>();

                        positionUpRight.Add(row + 2);

                        positionUpRight.Add(column + 2);

                        positionUpRight.Add(row + 1);

                        positionUpRight.Add(column + 1);

                        Debug.Log("able to eat again at +2+2");
                        checkIfHasToEat = true;
                    }
                }

            }
        }
        if (row < currentTable.GetPiecesPosition().Count - 1 && column > 0)
        {
            if (currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack() &&
                !piecesToEat.Contains(currentTable.GetPiecesPosition()[row + 1][column - 1]))
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
                currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack() &&
                !piecesToEat.Contains(currentTable.GetPiecesPosition()[row - 1][column + 1]))
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
                currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack() &&
                !piecesToEat.Contains(currentTable.GetPiecesPosition()[row - 1][column - 1]))
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

    public List<Piece> GetListOfPiecesAbleToEat()
    {
        if (piecesThatHasToEatInBigginingOfTurn == null)
        {
            piecesThatHasToEatInBigginingOfTurn = CheckForPiecesThatHasToEatInitaly();
        }
        return piecesThatHasToEatInBigginingOfTurn;
    }

    private List<Piece> CheckForPiecesThatHasToEatInitaly()
    {
        List<Piece> newListOfPiecesAbleToEat = new List<Piece>();

        /*Piece currentPieceClicked = GetClickedPiece().GetComponent<Piece>();*/
        int rowController = 0;
        int collumnController = 0;
        while (rowController < currentTable.GetCurretBoardSpacePositions()[0].Count)
        {
            if ((currentTable.GetPiecesPosition()[rowController][collumnController] != null) && (currentTable.GetPiecesPosition()[rowController][collumnController].GetIsBlack() == isBlackTurn))
            {
                bool canEatUpLeft = false;
                bool canEatUpRight = false;
                bool canEatDownLeft = false;
                bool canEatDownRight = false;
                bool isPieceInContactWihtOpponentUpLeft = PieceInContactUpLeft(rowController, collumnController, false);
                if (isPieceInContactWihtOpponentUpLeft == true && contactPositionUpLeft[1] > 0)
                {
                    canEatUpLeft = PieceInContactUpLeft(contactPositionUpLeft[0], contactPositionUpLeft[1], isPieceInContactWihtOpponentUpLeft);

                }
                bool isPieceInContactWihtOpponentUpRight = PieceInContactUpRight(rowController, collumnController, false);
                if (isPieceInContactWihtOpponentUpRight)
                {
                    canEatUpRight = PieceInContactUpRight(contacPositionUpRight[0], contacPositionUpRight[1], isPieceInContactWihtOpponentUpRight);
                }
                bool isPieceInContactWihtOpponentDownLeft = PieceInContactDownLeft(rowController, collumnController, false);
                if (isPieceInContactWihtOpponentDownLeft == true && contactPositionDownLeft[1] > 0)
                {
                    canEatDownLeft = PieceInContactDownLeft(contactPositionDownLeft[0], contactPositionDownLeft[1], isPieceInContactWihtOpponentDownLeft);

                }
                bool isPieceInContactWihtOpponentDownRight = PieceInContactDownRight(rowController, collumnController, false);
                if (isPieceInContactWihtOpponentDownRight)
                {
                    canEatDownRight = PieceInContactDownRight(contacPositionDownRight[0], contacPositionDownRight[1], isPieceInContactWihtOpponentDownRight);
                }
                if (canEatDownLeft == true || canEatDownRight == true || canEatUpLeft == true || canEatUpRight == true)
                {
                    newListOfPiecesAbleToEat.Add(currentTable.GetPiecesPosition()[rowController][collumnController]);
                }
            }

            if (collumnController < currentTable.GetCurretBoardSpacePositions()[0].Count - 1)
            {
                collumnController++;
            }
            else
            {
                collumnController = 0;
                rowController++;
            }

        }
        return newListOfPiecesAbleToEat;
    }

}
