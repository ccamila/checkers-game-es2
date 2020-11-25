using JetBrains.Annotations;
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

    public GameObject turnGO;

    private bool isPieceClicked = false;
    private bool mandatoryEat = false;
    private GameObject clickedPiece = null;
    CurrentTable currentTable;
    bool isBlackTurn = false;
    bool hasEaten = false;
    List<int> eatPositionDownLeft;
    List<int> eatPositionDownRight;
    List<int> eatPositionUpLeft;
    List<int> eatPositionUpRight;
    List<int> contactPositionDownLeft;
    List<int> contacPositionDownRight;
    List<int> contactPositionUpLeft;
    List<int> contactPositionUpRight;
    List<int> positionUpLeft;
    List<int> positionUpRight;
    List<int> positionDownLeft;
    List<int> positionDownRight;

    Dictionary<int, List<int>> positionToEatAgain;
    int dictionaryIndexController = 0;
    List<Piece> piecesToEat;
    List<List<int>> piecesToEatPositions;
    bool initialTurnEatLock = false;
    List<Piece> piecesThatHasToEatInBegginingOfTurn;

    Dictionary<int, List<int>> positionsKingMovePossibilities;
    int indexKingPositionController;

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
        iaPlayerController = IAPlayerController.Instance();
        tableConstructor.ConstructBoard();
        piecesConstructor.ConstructPieces();
        iaPlayerController.SetGameController();
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

    private void DoesAIMustPlay()
    {
        //        if( (isBlackTurn && iaPlayerController.GetIsIAPlayerBlack()) || (!isBlackTurn && !iaPlayerController.GetIsIAPlayerBlack()))
        //       {
        //           if (!iaPlayerController.MakeAMove())
        //           {
        //              Debug.Log("AI cant move");
        //          }

        //      }

    }
    public void UpdateTurnUI(bool isBlackTurn)
    {
        Debug.Log("Disparando turno do preto: " + isBlackTurn);
        //turnGO.GetComponent<Animator>().SetBool("isBlackTurn", isBlackTurn);
    }


    public void UpdateGameObject()
    {
        if (initialTurnEatLock == false)
        {
            if (pieceToUpdate && piecesThatHasToEatInBegginingOfTurn.Count > 0 && piecesThatHasToEatInBegginingOfTurn.Contains(pieceToUpdate))
            {
                piecesToEat = new List<Piece>();
                piecesToEatPositions = new List<List<int>>();
                positionsKingMovePossibilities = new Dictionary<int, List<int>>();
                indexKingPositionController = 0;
                bool canEatDownLeft = false;
                bool canEatDownRight = false;
                bool canEatUpLeft = false;
                bool canEatUpRight = false;

                bool isPieceInContactWihtOpponentDownLeft = PieceInContactDownLeft(currentPosition[0], currentPosition[1], false, false);
                if (isPieceInContactWihtOpponentDownLeft == true && contactPositionDownLeft[1] > 0)
                {
                    canEatDownLeft = PieceInContactDownLeft(contactPositionDownLeft[0], contactPositionDownLeft[1], isPieceInContactWihtOpponentDownLeft, false);

                }
                bool isPieceInContactWihtOpponentDownRight = PieceInContactDownRight(currentPosition[0], currentPosition[1], false, false);

                if (isPieceInContactWihtOpponentDownRight)
                {
                    canEatDownRight = PieceInContactDownRight(contacPositionDownRight[0], contacPositionDownRight[1], isPieceInContactWihtOpponentDownRight, false);
                }
                bool isPieceInContactWihtOpponentUpLeft = PieceInContactUpLeft(currentPosition[0], currentPosition[1], false, false);
                if (isPieceInContactWihtOpponentUpLeft == true && contactPositionUpLeft[1] > 0)
                {
                    canEatUpLeft = PieceInContactUpLeft(contactPositionUpLeft[0], contactPositionUpLeft[1], isPieceInContactWihtOpponentUpLeft, false);
                }
                bool isPieceInContactWihtOpponentUpRight = PieceInContactUpRight(currentPosition[0], currentPosition[1], false, false);

                if (isPieceInContactWihtOpponentUpRight)
                {
                    canEatUpRight = PieceInContactUpRight(contactPositionUpRight[0], contactPositionUpRight[1], isPieceInContactWihtOpponentUpRight, false);
                }
                if (pieceToUpdate.GetKingStatus() == false)
                {
                    if (canEatDownLeft && (newPosition[0] == eatPositionDownLeft[0] && newPosition[1] == eatPositionDownLeft[1]))
                    {
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]]))
                        {
                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionDownLeft[0], contactPositionDownLeft[1]);
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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                piecesToEat = new List<Piece>();
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionDownLeft[0] + " " + contactPositionDownLeft[1]);
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

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contacPositionDownRight[0], contacPositionDownRight[1]);

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

                            currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].SetIsAvaiableToEat(false);

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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contacPositionDownRight[0] + " " + contacPositionDownRight[1]);
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
                    else if (canEatUpLeft && (newPosition[0] == eatPositionUpLeft[0] && newPosition[1] == eatPositionUpLeft[1]))
                    {
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]]))
                        {

                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionUpLeft[0], contactPositionUpLeft[1]);

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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionUpLeft[0] + " " + contactPositionUpLeft[1]);
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
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]]))
                        {

                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionUpRight[0], contactPositionUpRight[1]);

                            List<int> auxiliarPositiontoEatList = new List<int>();
                            auxiliarPositiontoEatList.Add(contactPositionUpRight[0]);
                            auxiliarPositiontoEatList.Add(contactPositionUpRight[1]);
                            piecesToEat.Add(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]]);
                            if (positionToEatAgain == null)
                            {
                                positionToEatAgain = new Dictionary<int, List<int>>();
                                dictionaryIndexController = 0;
                            }
                            positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                            dictionaryIndexController++;

                            bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                            currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].SetIsAvaiableToEat(false);

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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionUpRight[0] + " " + contactPositionUpRight[1]);
                                currentTable.UpdatePiecesPosition(contactPositionUpRight[0], contactPositionUpRight[1], null);
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
                else
                {

                    if (canEatDownLeft && (newPosition[0] == eatPositionDownLeft[0] && newPosition[1] == eatPositionDownLeft[1]))
                    {
                        Debug.Log(eatPositionDownLeft[0] + "DL");
                        Debug.Log(eatPositionDownLeft[1] + "DL");
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]]))
                        {
                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
                            pieceToUpdate.gameObject.transform.position = newBoardPosition;
                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionDownLeft[0], contactPositionDownLeft[1]);
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
                            Debug.Log(checkToEatAgain + " EATT again dwn LEF");
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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                piecesToEat = new List<Piece>();
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionDownLeft[0]][contactPositionDownLeft[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionDownLeft[0] + " " + contactPositionDownLeft[1]);
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
                        Debug.Log(eatPositionDownRight[0] + "Dr");
                        Debug.Log(eatPositionDownRight[1] + "Dr");
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]]))
                        {
                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contacPositionDownRight[0], contacPositionDownRight[1]);

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

                            currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].SetIsAvaiableToEat(false);

                            bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);
                            Debug.Log(checkToEatAgain + " EATT again down RIG");
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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contacPositionDownRight[0]][contacPositionDownRight[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contacPositionDownRight[0] + " " + contacPositionDownRight[1]);
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
                    else if (canEatUpLeft && (newPosition[0] == eatPositionUpLeft[0] && newPosition[1] == eatPositionUpLeft[1]))
                    {
                        Debug.Log(eatPositionUpLeft[0] + "UL");
                        Debug.Log(eatPositionUpLeft[1] + "UL");
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]]))
                        {

                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionUpLeft[0], contactPositionUpLeft[1]);

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
                            Debug.Log(checkToEatAgain + " EATT again up LEF");
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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionUpLeft[0]][contactPositionUpLeft[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionUpLeft[0] + " " + contactPositionUpLeft[1]);
                                Debug.Log(contactPositionUpLeft[0] + " CONTAAAAAAAAAAAAAAAAAAAAAAAAAACT");
                                Debug.Log(contactPositionUpLeft[1] + " CONTAAAAAAAAAAAAAAAAAAAAAAAAAACT");
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
                        Debug.Log(eatPositionUpRight[0] + "Ur");
                        Debug.Log(eatPositionUpRight[1] + "Ur");
                        if (!piecesToEat.Contains(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]]))
                        {

                            GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;
                            Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

                            pieceToUpdate.gameObject.transform.position = newBoardPosition;

                            currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);

                            currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                            currentPosition[0] = newPosition[0];
                            currentPosition[1] = newPosition[1];

                            currentTable.GetCurrentBoard().SetBoardTilePlayable(contactPositionUpRight[0], contactPositionUpRight[1]);

                            List<int> auxiliarPositiontoEatList = new List<int>();
                            auxiliarPositiontoEatList.Add(contactPositionUpRight[0]);
                            auxiliarPositiontoEatList.Add(contactPositionUpRight[1]);
                            piecesToEat.Add(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]]);
                            if (positionToEatAgain == null)
                            {
                                positionToEatAgain = new Dictionary<int, List<int>>();
                                dictionaryIndexController = 0;
                            }
                            positionToEatAgain.Add(dictionaryIndexController, auxiliarPositiontoEatList);
                            dictionaryIndexController++;

                            bool checkToEatAgain = CheckPieceDiagonals(newPosition[0], newPosition[1]);

                            currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].SetIsAvaiableToEat(false);

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
                                piecesThatHasToEatInBegginingOfTurn = null;
                                Debug.Log("Eating piece " + currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].gameObject.name);
                                Destroy(currentTable.GetPiecesPosition()[contactPositionUpRight[0]][contactPositionUpRight[1]].gameObject);
                                Debug.Log("DESTROYING PIECE AT " + contactPositionUpRight[0] + " " + contactPositionUpRight[1]);
                                currentTable.UpdatePiecesPosition(contactPositionUpRight[0], contactPositionUpRight[1], null);
                                positionToEatAgain = new Dictionary<int, List<int>>();
                                dictionaryIndexController = 0;
                            }
                        }
                        else
                        {
                            Debug.Log("jogada morre aqui, liberar ");
                        }
                    }
                }
            }

            else if (pieceToUpdate && piecesThatHasToEatInBegginingOfTurn.Count == 0)
            {
                if (pieceToUpdate.GetKingStatus() == false)
                {

                    GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;

                    if (((pieceToUpdate.GetIsUp() == true && ((currentPosition[0] - newPosition[0]) == 1)) ||
                        (pieceToUpdate.GetIsUp() == false && (newPosition[0] - currentPosition[0]) == 1)) &&
                        ((currentPosition[1] - newPosition[1] == 1) || (currentPosition[1] - newPosition[1] == -1)))
                    {
                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);

                        pieceToUpdate.gameObject.transform.position = newBoardPosition;
                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];

                        if ((pieceToUpdate.GetKingStatus() == false && pieceToUpdate.GetIsUp() == false && currentPosition[0] == currentTable.GetCurrentBoard().GetSizeOfTable() - 1)
                            || (pieceToUpdate.GetKingStatus() == false && pieceToUpdate.GetIsUp() == true && currentPosition[0] == 0))
                        {
                            pieceToUpdate.SetKing(true);

                        }
                        pieceToUpdate = null;
                        SetIsPieceClicked();
                        SetClickedPiece(null);
                        isBlackTurn = !isBlackTurn;
                        piecesThatHasToEatInBegginingOfTurn = null;

                    }
                    else
                    {
                        Debug.Log("Cannot Move to there now");
                    }
                }
                else
                {

                    List<int> auxiliarPositionList = new List<int>();
                    auxiliarPositionList.Add(newPosition[0]);
                    auxiliarPositionList.Add(newPosition[1]);

                    bool compareValues = false;
                    for (int i = 0; i < positionsKingMovePossibilities.Count; i++)
                    {
                        List<int> positionListToCompare = positionsKingMovePossibilities[i];

                        if (positionListToCompare[0] == newPosition[0] && positionListToCompare[1] == newPosition[1])
                        {
                            compareValues = true;
                        }
                    }
                    if (compareValues == true)
                    {
                        GameObject newBoardPositionPiece = currentTable.GetCurretBoardSpacePositions()[newPosition[0]][newPosition[1]].gameObject;

                        Vector2 newBoardPosition = new Vector2(newBoardPositionPiece.transform.position.x, newBoardPositionPiece.transform.position.y);
                        pieceToUpdate.gameObject.transform.position = newBoardPosition;
                        currentTable.UpdatePiecesPosition(newPosition[0], newPosition[1], pieceToUpdate);

                        currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                        currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                        currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
                        currentPosition[0] = newPosition[0];
                        currentPosition[1] = newPosition[1];

                        if ((pieceToUpdate.GetKingStatus() == false && pieceToUpdate.GetIsUp() == false && currentPosition[0] == currentTable.GetCurrentBoard().GetSizeOfTable() - 1)
                            || (pieceToUpdate.GetKingStatus() == false && pieceToUpdate.GetIsUp() == true && currentPosition[0] == 0))
                        {

                            pieceToUpdate.SetKing(true);
                            Debug.Log($"Curret piece {pieceToUpdate.gameObject.name} is now king");
                        }
                        pieceToUpdate = null;
                        SetIsPieceClicked();
                        SetClickedPiece(null);
                        isBlackTurn = !isBlackTurn;
                        piecesThatHasToEatInBegginingOfTurn = null;

                    }
                    else
                    {
                        Debug.Log("Cannot Move to there now");
                    }
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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);

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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionUpLeft[2], positionUpLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardTilePlayable(positionUpLeft[2], positionUpLeft[3]);

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
                    piecesThatHasToEatInBegginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(boardPieceList[0], boardPieceList[1], true);

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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionUpRight[2], positionUpRight[3], null);
                currentTable.GetCurrentBoard().SetBoardTilePlayable(positionUpRight[2], positionUpRight[3]);

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
                    piecesThatHasToEatInBegginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(boardPieceList[0], boardPieceList[1], true);

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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownLeft[2], positionDownLeft[3], null);
                currentTable.GetCurrentBoard().SetBoardTilePlayable(positionDownLeft[2], positionDownLeft[3]);

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
                    piecesThatHasToEatInBegginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(boardPieceList[0], boardPieceList[1], true);
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

                currentTable.GetCurrentBoard().SetBoardTilePlayable(currentPosition[0], currentPosition[1]);
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
                currentTable.GetCurrentBoard().SetBoardTilePlayable(newPosition[0], newPosition[1], false);
                currentTable.UpdatePiecesPosition(currentPosition[0], currentPosition[1], null);
                currentTable.UpdatePiecesPosition(positionDownRight[2], positionDownRight[3], null);
                currentTable.GetCurrentBoard().SetBoardTilePlayable(positionDownRight[2], positionDownRight[3]);

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
                    piecesThatHasToEatInBegginingOfTurn = null;
                    if (piecesToEat.Count > 0)
                    {
                        for (int i = 0; i < piecesToEat.Count; i++)
                        {
                            Debug.Log(piecesToEat[i].gameObject.name);
                            Destroy(piecesToEat[i].gameObject);
                            List<int> boardPieceList = new List<int>();
                            boardPieceList = positionToEatAgain[i];
                            currentTable.GetCurrentBoard().SetBoardTilePlayable(boardPieceList[0], boardPieceList[1], true);

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
    private bool PieceInContactDownLeft(int row, int column, bool hasContatc, bool isSimulated)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (isSimulated == true)

        {
            if (row > 0 && column > 0)
            {
                Piece currentPiece = currentTable.GetPiecesPosition()[row][column];
                Debug.Log(row + " roww AAAAAAAAAAAAAAAAAAA");
                Debug.Log(column + " colu  AAAAAAAAAAAAAAAAAAA");

                if (currentPiece.GetKingStatus() == false)
                {

                    if (hasContatc == false)
                    {
                        if (row - 1 >= 0 && column - 1 >= 0 && currentTable.GetPiecesPosition()[row - 1][column - 1] != null &&
                            currentTable.GetPiecesPosition()[row - 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                        {
                            contactPositionDownLeft = new List<int>();
                            Debug.Log("initiating");
                            contactPositionDownLeft.Add(row - 1);
                            contactPositionDownLeft.Add(column - 1);
                            isPieceInContact = true;
                        }
                    }
                    else
                    {
                        if (row - 1 >= 0 && column - 1 >= 0 && currentTable.GetPiecesPosition()[row - 1][column - 1] == null)
                        {
                            eatPositionDownLeft = new List<int>();
                            eatPositionDownLeft.Add(row - 1);
                            eatPositionDownLeft.Add(column - 1);
                            canEat = true;
                        }
                    }
                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;
                        int valueControl = row;
                        if (column < row)
                        {
                            valueControl = column;
                        }
                        while (valueControl > 0)
                        {

                            if (row - diagonalController >= 0 && column - diagonalController >= 0 && currentTable.GetPiecesPosition()[row - diagonalController][column - diagonalController] != null &&
                       currentTable.GetPiecesPosition()[row - diagonalController][column - diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                contactPositionDownLeft = new List<int>();
                                contactPositionDownLeft.Add(row - diagonalController);
                                contactPositionDownLeft.Add(column - diagonalController);
                                isPieceInContact = true;
                                valueControl = 0;
                                Debug.Log("aqui estou ");
                                //diagonalCheck = false;
                            }
                            else if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] - diagonalController >= 0 &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController] != null &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {
                                valueControl = 0;
                            }

                            else if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] - diagonalController >= 0)
                            {
                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(row - diagonalController);
                                auxPositions.Add(column - diagonalController);
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueControl--;
                            }
                            else
                            {
                                diagonalController++;
                                valueControl--;
                            }
                        }
                    }
                    else
                    {
                        if (row - 1 >= 0 && column - 1 >= 0 && currentTable.GetPiecesPosition()[row - 1][column - 1] == null)
                        {
                            eatPositionDownLeft = new List<int>();
                            eatPositionDownLeft.Add(row - 1);
                            eatPositionDownLeft.Add(column - 1);
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
        }
        else
        {
            if (currentPosition[1] > 0 && currentPosition[0] > 0)
            {
                if (pieceToUpdate.GetKingStatus() == false)
                {

                    if (hasContatc == false)
                    {
                        if (currentPosition[0] - 1 >= 0 && currentPosition[1] - 1 >= 0 && currentTable.GetPiecesPosition()[currentPosition[0] - 1][currentPosition[1] - 1] != null &&
                            currentTable.GetPiecesPosition()[currentPosition[0] - 1][currentPosition[1] - 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                        {

                            contactPositionDownLeft = new List<int>();
                            Debug.Log("Here");
                            contactPositionDownLeft.Add(currentPosition[0] - 1);
                            contactPositionDownLeft.Add(currentPosition[1] - 1);
                            isPieceInContact = true;
                        }
                    }
                    else
                    {
                        if (contactPositionDownLeft[0] - 1 >= 0 && contactPositionDownLeft[1] - 1 >= 0 && currentTable.GetPiecesPosition()[contactPositionDownLeft[0] - 1][contactPositionDownLeft[1] - 1] == null)
                        {
                            eatPositionDownLeft = new List<int>();
                            eatPositionDownLeft.Add(contactPositionDownLeft[0] - 1);
                            eatPositionDownLeft.Add(contactPositionDownLeft[1] - 1);

                            Debug.Log("CANT EAT AT ROW " + (contactPositionDownLeft[0] - 1));
                            Debug.Log("CANT EAT AT COL " + (contactPositionDownLeft[1] - 1));

                            canEat = true;
                        }
                    }
                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;

                        int valueControl = currentPosition[0];
                        if (currentPosition[1] < currentPosition[0])
                        {
                            valueControl = column;
                        }
                        while (valueControl > 0)
                        {

                            if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] - diagonalController >= 0 &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController] != null &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {
                                Debug.Log("INITIAL HERE");
                                contactPositionDownLeft = new List<int>();
                                contactPositionDownLeft.Add(currentPosition[0] - diagonalController);
                                contactPositionDownLeft.Add(currentPosition[1] - diagonalController);
                                isPieceInContact = true;
                                valueControl = 0;
                                //diagonalCheck = false;
                            }
                            else if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] - diagonalController >= 0 &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController] != null &&
                                currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] - diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {
                                valueControl = 0;
                            }
                            else if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] - diagonalController >= 0)
                            {
                                Debug.Log("Third HERE");
                                Debug.Log(currentPosition[0] - diagonalController + " Row not simulated");
                                Debug.Log(currentPosition[1] - diagonalController + " Column not simulated");
                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(currentPosition[0] - diagonalController);
                                auxPositions.Add(currentPosition[1] - diagonalController);
                                Debug.Log(positionsKingMovePossibilities.Count + " MMNN V ");
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);

                                diagonalController++;
                                valueControl--;

                            }
                            else
                            {
                                diagonalController++;
                                valueControl--;
                            }
                        }
                    }
                    else
                    {
                        if (contactPositionDownLeft[0] - 1 >= 0 && contactPositionDownLeft[1] - 1 >= 0 && currentTable.GetPiecesPosition()[contactPositionDownLeft[0] - 1][contactPositionDownLeft[1] - 1] == null)
                        {
                            eatPositionDownLeft = new List<int>();
                            eatPositionDownLeft.Add(contactPositionDownLeft[0] - 1);
                            eatPositionDownLeft.Add(contactPositionDownLeft[1] - 1);
                            canEat = true;
                            Debug.Log("OK TO KING EAT at down left");

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
        }

        return finalCheck;
    }

    private bool PieceInContactDownRight(int row, int column, bool hasContatc, bool isSimulated)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (isSimulated == true)
        {

            if (column < currentTable.GetPiecesPosition()[currentPosition[1]].Count - 1 && row > 0)
            {
                Piece currentPiece = currentTable.GetPiecesPosition()[row][column];
                if (currentPiece.GetKingStatus() == false)
                {
                    if (hasContatc == false)
                    {
                        if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row - 1 >= 0 &&
                            currentTable.GetPiecesPosition()[row - 1][column + 1] != null &&
                           currentTable.GetPiecesPosition()[row - 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                        {
                            contacPositionDownRight = new List<int>();
                            contacPositionDownRight.Add(row - 1);
                            contacPositionDownRight.Add(column + 1);
                            isPieceInContact = true;
                        }
                    }
                    else
                    {
                        if (column + 1 < currentTable.GetPiecesPosition()[0].Count && row - 1 >= 0 &&
                            currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
                        {
                            eatPositionDownRight = new List<int>();
                            eatPositionDownRight.Add(row - 1);
                            eatPositionDownRight.Add(column + 1);
                            canEat = true;
                        }
                    }
                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;
                        int valueRowControl = row;
                        int valueColumnControl = column;

                        while (valueRowControl >= 0 && valueColumnControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                        {
                            /*                        Debug.Log(row - diagonalController + "   ROW");
                                                    Debug.Log(column - diagonalController + "    COLLUMN");*/
                            if (row - diagonalController >= 0 && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController] != null &&
                           currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                contacPositionDownRight = new List<int>();
                                contacPositionDownRight.Add(row - diagonalController);
                                contacPositionDownRight.Add(column + diagonalController);
                                isPieceInContact = true;
                                valueRowControl = 0;
                                valueColumnControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                //diagonalCheck = false;
                            }
                            else if (row - diagonalController >= 0 && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController] != null &&
                                currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                valueRowControl = 0;
                                valueColumnControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                            }
                            else if (row - diagonalController >= 0 && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                            {
                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(row - diagonalController);
                                auxPositions.Add(column + diagonalController);
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl--;

                            }
                            else
                            {
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl--;
                            }
                        }
                    }
                    else
                    {
                        if (row - 1 >= 0 && column + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
                        {
                            eatPositionDownRight = new List<int>();
                            eatPositionDownRight.Add(row - 1);
                            eatPositionDownRight.Add(column + 1);
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
        }
        else
        {
            if (currentPosition[1] < currentTable.GetPiecesPosition()[currentPosition[1]].Count - 1 && currentPosition[0] > 0)
            {
                if (pieceToUpdate.GetKingStatus() == false)
                {

                    if (hasContatc == false)
                    {

                        if (currentPosition[1] + 1 < currentTable.GetPiecesPosition()[0].Count && currentPosition[0] - 1 >= 0 &&
                            currentTable.GetPiecesPosition()[currentPosition[0] - 1][currentPosition[1] + 1] != null &&
                           currentTable.GetPiecesPosition()[currentPosition[0] - 1][currentPosition[1] + 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                        {

                            contacPositionDownRight = new List<int>();
                            contacPositionDownRight.Add(currentPosition[0] - 1);
                            contacPositionDownRight.Add(currentPosition[1] + 1);
                            isPieceInContact = true;
                        }
                    }
                    else
                    {
                        if (contacPositionDownRight[1] + 1 < currentTable.GetPiecesPosition()[0].Count && contacPositionDownRight[0] - 1 >= 0 &&
                            currentTable.GetPiecesPosition()[contacPositionDownRight[0] - 1][contacPositionDownRight[1] + 1] == null)
                        {
                            Debug.Log("HRT CAN EAT d r 0");
                            eatPositionDownRight = new List<int>();
                            eatPositionDownRight.Add(contacPositionDownRight[0] - 1);
                            eatPositionDownRight.Add(contacPositionDownRight[1] + 1);
                            canEat = true;
                        }
                    }
                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;
                        int valueRowControl = currentPosition[0];
                        int valueColumnControl = currentPosition[1];

                        while (valueRowControl >= 0 && valueColumnControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                        {
                            /*                        Debug.Log(row - diagonalController + "   ROW");
                                                    Debug.Log(column - diagonalController + "    COLLUMN");*/
                            if (currentPosition[0] - diagonalController >= 0 && currentPosition[1] + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] + diagonalController] != null &&
                           currentTable.GetPiecesPosition()[currentPosition[0] - diagonalController][currentPosition[1] + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {
                                contacPositionDownRight = new List<int>();
                                contacPositionDownRight.Add(currentPosition[0] - diagonalController);
                                contacPositionDownRight.Add(currentPosition[1] + diagonalController);
                                isPieceInContact = true;
                                valueRowControl = 0;
                                valueColumnControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                //diagonalCheck = false;
                            }
                            else if (row - diagonalController >= 0 && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController] != null &&
                                currentTable.GetPiecesPosition()[row - diagonalController][column + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                valueRowControl = 0;
                                valueColumnControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                            }
                            else if (row - diagonalController >= 0 && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                            {

                                Debug.Log(currentPosition[0] - diagonalController + " Row not simulated");
                                Debug.Log(currentPosition[1] + diagonalController + " Column not simulated");
                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(currentPosition[0] - diagonalController);
                                auxPositions.Add(currentPosition[1] + diagonalController);
                                Debug.Log(positionsKingMovePossibilities.Count + " MMNN V ");
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl--;

                            }
                            else
                            {
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl--;
                            }
                        }
                    }
                    else
                    {
                        if (currentPosition[0] - 1 >= 0 && currentPosition[1] + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[currentPosition[0] - 1][currentPosition[1] + 1] == null)
                        {
                            eatPositionDownRight = new List<int>();
                            eatPositionDownRight.Add(contacPositionDownRight[0] - 1);
                            eatPositionDownRight.Add(contacPositionDownRight[1] + 1);
                            canEat = true;
                            Debug.Log("OK TO KING EAT at dow right");

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
        }
        return finalCheck;
    }

    private bool PieceInContactUpLeft(int row, int column, bool hasContatc, bool isSimulated)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (isSimulated == true)
        {
            if (column > 0 && row < currentTable.GetPiecesPosition()[currentPosition[1]].Count)
            {
                Piece currentPiece = currentTable.GetPiecesPosition()[row][column];
                Debug.Log(hasContatc);
                Debug.Log(row);
                Debug.Log(column);
                Debug.Log("ASSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");

                if (currentPiece.GetKingStatus() == false)
                {

                    if (hasContatc == false)
                    {
                        if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] != null &&
                        currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                        {
                            contactPositionUpLeft = new List<int>();
                            contactPositionUpLeft.Add(row + 1);
                            contactPositionUpLeft.Add(column - 1);
                            isPieceInContact = true;
                            Debug.Log(currentTable.GetPiecesPosition()[row + 1][column - 1].GetIsBlack());
                            Debug.Log(currentTable.GetPiecesPosition()[row][column].GetIsBlack());
                            Debug.Log("THE ROW IN CONTACT " + row);
                            Debug.Log("THE Col IN CONTACT " + column);
                        }
                    }
                    else
                    {
                        if (row + 1 < currentTable.GetPiecesPosition().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] == null)
                        {
                            eatPositionUpLeft = new List<int>();
                            eatPositionUpLeft.Add(row + 1);
                            eatPositionUpLeft.Add(column - 1);
                            canEat = true;
                        }
                    }

                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;
                        int valueRowControl = row;
                        int valueColumnControl = column;

                        while (valueRowControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count && valueColumnControl >= 0)
                        {
                            Debug.Log("OK OK OK OK OK O ENTRE HIW ERWEW");

                            if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - diagonalController >= 0 && currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController] != null &&
                           currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                contactPositionUpLeft = new List<int>();
                                contactPositionUpLeft.Add(row + diagonalController);
                                contactPositionUpLeft.Add(column - diagonalController);
                                isPieceInContact = true;
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;

                            }

                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - diagonalController >= 0 && currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController] != null &&
                           currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;
                            }
                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - diagonalController >= 0)
                            {

                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(row + diagonalController);
                                auxPositions.Add(column - diagonalController);
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueColumnControl--;
                                valueRowControl++;

                            }
                            else
                            {
                                diagonalController++;
                                valueColumnControl--;
                                valueRowControl++;
                            }
                        }
                    }
                    else
                    {
                        if (row + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - 1 >= 0 && currentTable.GetPiecesPosition()[row + 1][column - 1] == null)
                        {
                            eatPositionUpLeft = new List<int>();
                            eatPositionUpLeft.Add(row - 1);
                            eatPositionUpLeft.Add(column + 1);
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
        }
        else
        {
            if (currentPosition[1] > 0 && currentPosition[0] < currentTable.GetPiecesPosition()[currentPosition[0]].Count)
            {
                if (pieceToUpdate.GetKingStatus() == false)
                {
                    if (hasContatc == false)
                    {
                        if (currentPosition[0] + 1 < currentTable.GetPiecesPosition().Count && currentPosition[1] - 1 >= 0 && currentTable.GetPiecesPosition()[currentPosition[0] + 1][currentPosition[1] - 1] != null &&
                        currentTable.GetPiecesPosition()[currentPosition[0] + 1][currentPosition[1] - 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                        {
                            contactPositionUpLeft = new List<int>();
                            contactPositionUpLeft.Add(currentPosition[0] + 1);
                            contactPositionUpLeft.Add(currentPosition[1] - 1);
                            isPieceInContact = true;
                        }
                    }
                    else
                    {
                        if (contactPositionUpLeft[0] + 1 < currentTable.GetPiecesPosition().Count && contactPositionUpLeft[1] - 1 >= 0
                            && currentTable.GetPiecesPosition()[contactPositionUpLeft[0] + 1][contactPositionUpLeft[1] - 1] == null)
                        {
                            eatPositionUpLeft = new List<int>();
                            eatPositionUpLeft.Add(contactPositionUpLeft[0] + 1);
                            eatPositionUpLeft.Add(contactPositionUpLeft[1] - 1);
                            canEat = true;
                        }
                    }

                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;
                        int valueRowControl = currentPosition[0];
                        int valueColumnControl = currentPosition[1];

                        while (valueRowControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count && valueColumnControl >= 0)
                        {

                            if (currentPosition[0] + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentPosition[1] - diagonalController >= 0 && currentTable.GetPiecesPosition()[currentPosition[0] + diagonalController][currentPosition[1] - diagonalController] != null &&
                           currentTable.GetPiecesPosition()[currentPosition[0] + diagonalController][currentPosition[1] - diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {
                                contactPositionUpLeft = new List<int>();
                                contactPositionUpLeft.Add(currentPosition[0] + diagonalController);
                                contactPositionUpLeft.Add(currentPosition[1] - diagonalController);
                                isPieceInContact = true;
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;
                                //diagonalCheck = false;
                            }

                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - diagonalController >= 0 && currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController] != null &&
                           currentTable.GetPiecesPosition()[row + diagonalController][column - diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;
                            }

                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column - diagonalController >= 0)
                            {

                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(currentPosition[0] + diagonalController);
                                auxPositions.Add(currentPosition[1] - diagonalController);
                                Debug.Log(positionsKingMovePossibilities.Count + " MMNN V ");
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueColumnControl--;
                                valueRowControl++;

                            }
                            else
                            {
                                diagonalController++;
                                valueColumnControl--;
                                valueRowControl++;
                            }
                        }
                    }
                    else
                    {
                        if (contactPositionUpLeft[0] + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && contactPositionUpLeft[1] - 1 >= 0 && currentTable.GetPiecesPosition()[contactPositionUpLeft[0] + 1][contactPositionUpLeft[1] - 1] == null)
                        {
                            eatPositionUpLeft = new List<int>();
                            eatPositionUpLeft.Add(contactPositionUpLeft[0] - 1);
                            eatPositionUpLeft.Add(contactPositionUpLeft[1] + 1);
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
        }

        return finalCheck;
    }

    private bool PieceInContactUpRight(int row, int column, bool hasContatc, bool isSimulated)
    {
        bool finalCheck = false;
        bool isPieceInContact = false;
        bool canEat = false;

        if (isSimulated == true)
        {
            if (row < currentTable.GetPiecesPosition()[0].Count - 1 && column < currentTable.GetPiecesPosition()[0].Count)
            {
                Piece currentPiece = currentTable.GetPiecesPosition()[row][column];
                if (currentPiece.GetKingStatus() == false)
                {
                    if (hasContatc == false)
                    {
                        if (row + 1 < currentTable.GetPiecesPosition()[0].Count &&
                            column + 1 < currentTable.GetPiecesPosition()[0].Count &&
                            currentTable.GetPiecesPosition()[row + 1][column + 1] != null &&
                            currentTable.GetPiecesPosition()[row + 1][column + 1].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                        {

                            contactPositionUpRight = new List<int>();
                            contactPositionUpRight.Add(row + 1);
                            contactPositionUpRight.Add(column + 1);
                            Debug.Log(contactPositionUpRight[0] + " UR row");
                            Debug.Log(contactPositionUpRight[1] + " UR COL");


                            isPieceInContact = true;

                        }
                    }
                    else
                    {
                        if (row + 1 < currentTable.GetPiecesPosition()[0].Count &&
                            column + 1 < currentTable.GetPiecesPosition()[0].Count &&
                            currentTable.GetPiecesPosition()[row + 1][column + 1] == null)
                        {
                            eatPositionUpRight = new List<int>();
                            eatPositionUpRight.Add(row + 1);
                            eatPositionUpRight.Add(column + 1);
                            canEat = true;
                        }
                    }

                }
                else
                {
                    if (hasContatc == false)
                    {
                        int diagonalController = 1;

                        int valueRowControl = row;
                        int valueColumnControl = column;

                        while (valueRowControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count && valueColumnControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                        {

                            if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController] != null &&
                           currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                contactPositionUpRight = new List<int>();
                                contactPositionUpRight.Add(row + diagonalController);
                                contactPositionUpRight.Add(column + diagonalController);
                                Debug.Log(contactPositionUpRight[0] + " UR row");
                                Debug.Log(contactPositionUpRight[1] + " UR COL");
                                isPieceInContact = true;
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;

                            }
                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController] != null &&
                                currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                            {
                                valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                valueColumnControl = 0;
                            }
                            else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                            {
                                List<int> auxPositions = new List<int>();
                                auxPositions.Add(row + diagonalController);
                                auxPositions.Add(column + diagonalController);
                                positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                indexKingPositionController++;
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl++;

                            }
                            else
                            {
                                diagonalController++;
                                valueColumnControl++;
                                valueRowControl++;
                            }
                        }
                    }
                    else
                    {
                        if (row + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row - 1][column + 1] == null)
                        {
                            eatPositionUpRight = new List<int>();
                            eatPositionUpRight.Add(row + 1);
                            eatPositionUpRight.Add(column + 1);
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
        }
        else
        {
            {
                if (currentPosition[0] < currentTable.GetPiecesPosition()[currentPosition[1]].Count && currentPosition[1] < currentTable.GetPiecesPosition()[currentPosition[1]].Count)
                {
                    if (pieceToUpdate.GetKingStatus() == false)
                    {
                        if (hasContatc == false)
                        {
                            if (currentPosition[0] + 1 < currentTable.GetPiecesPosition()[0].Count &&
                                currentPosition[1] + 1 < currentTable.GetPiecesPosition()[0].Count &&
                                currentTable.GetPiecesPosition()[currentPosition[0] + 1][currentPosition[1] + 1] != null &&
                                currentTable.GetPiecesPosition()[currentPosition[0] + 1][currentPosition[1] + 1].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                            {

                                contactPositionUpRight = new List<int>();
                                contactPositionUpRight.Add(currentPosition[0] + 1);
                                contactPositionUpRight.Add(currentPosition[1] + 1);

                                isPieceInContact = true;

                            }
                        }
                        else
                        {
                            if (contactPositionUpRight[0] + 1 < currentTable.GetPiecesPosition()[0].Count &&
                                contactPositionUpRight[1] + 1 < currentTable.GetPiecesPosition()[0].Count &&
                                currentTable.GetPiecesPosition()[contactPositionUpRight[0] + 1][contactPositionUpRight[1] + 1] == null)
                            {
                                eatPositionUpRight = new List<int>();
                                eatPositionUpRight.Add(contactPositionUpRight[0] + 1);
                                eatPositionUpRight.Add(contactPositionUpRight[1] + 1);
                                canEat = true;
                            }
                        }

                    }
                    else
                    {
                        if (hasContatc == false)
                        {
                            int diagonalController = 1;
                            int valueRowControl = currentPosition[0] + 1;
                            int valueColumnControl = currentPosition[1] + 1;

                            while (valueRowControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count && valueColumnControl < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                            {
                                /*                        Debug.Log(row - diagonalController + "   ROW");
                                                        Debug.Log(column - diagonalController + "    COLLUMN");*/
                                if (currentPosition[0] + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentPosition[1] + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[currentPosition[0] + diagonalController][currentPosition[1] + diagonalController] != null &&
                               currentTable.GetPiecesPosition()[currentPosition[0] + diagonalController][currentPosition[1] + diagonalController].GetIsBlack() != currentTable.GetPiecesPosition()[currentPosition[0]][currentPosition[1]].GetIsBlack())
                                {
                                    contactPositionUpRight = new List<int>();
                                    contactPositionUpRight.Add(currentPosition[0] + diagonalController);
                                    contactPositionUpRight.Add(currentPosition[1] + diagonalController);
                                    isPieceInContact = true;
                                    valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                    valueColumnControl = 0;
                                }
                                else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController] != null &&
                                    currentTable.GetPiecesPosition()[row + diagonalController][column + diagonalController].GetIsBlack() == currentTable.GetPiecesPosition()[row][column].GetIsBlack())
                                {
                                    valueRowControl = currentTable.GetCurrentBoard().GetBoardMatrix().Count;
                                    valueColumnControl = 0;
                                }
                                else if (row + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count && column + diagonalController < currentTable.GetCurrentBoard().GetBoardMatrix().Count)
                                {
                                    //Debug.Log(currentPosition[0] + diagonalController + " Row not simulated");
                                    //Debug.Log(currentPosition[1] + diagonalController + " Column not simulated");
                                    List<int> auxPositions = new List<int>();
                                    auxPositions.Add(currentPosition[0] + diagonalController);
                                    auxPositions.Add(currentPosition[1] + diagonalController);
                                    //Debug.Log(positionsKingMovePossibilities.Count + " MMNN V ");
                                    positionsKingMovePossibilities.Add(positionsKingMovePossibilities.Count, auxPositions);
                                    indexKingPositionController++;
                                    diagonalController++;
                                    valueColumnControl++;
                                    valueRowControl++;

                                }
                                else
                                {
                                    diagonalController++;
                                    valueColumnControl++;
                                    valueRowControl++;
                                }
                            }
                        }
                        else
                        {
                            if (contactPositionUpRight[0] + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && contactPositionUpRight[1] + 1 < currentTable.GetCurrentBoard().GetBoardMatrix().Count && currentTable.GetPiecesPosition()[contactPositionUpRight[0] + 1][contactPositionUpRight[1] + 1] == null)
                            {
                                eatPositionUpRight = new List<int>();
                                eatPositionUpRight.Add(contactPositionUpRight[0] + 1);
                                eatPositionUpRight.Add(contactPositionUpRight[1] + 1);
                                canEat = true;
                                Debug.Log("OK TO KING EAT at " + (contactPositionUpRight[0]) + " " + (contactPositionUpRight[1]));

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
            }
        }
        return finalCheck;
    }



    private bool CheckPieceDiagonals(int row, int column)
    {
        Debug.Log("ROW VALUEEEEEEEEEEEEEEEEEEEEEEE NEg" + row);
        Debug.Log("COL VALUEEEEEEEEEEEEEEEEEEEEEEE NEg" + column);
        bool checkIfHasToEat = false;
        if (pieceToUpdate.GetKingStatus() == false)
        {
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
        }
        else
        {
            int diagonaValueControl = 1;
            bool diagonalWhileControl = true;

            while (diagonalWhileControl)
            {
                if (row < currentTable.GetPiecesPosition().Count - diagonaValueControl && column < currentTable.GetPiecesPosition().Count - diagonaValueControl)
                {
                    if (currentTable.GetPiecesPosition()[row + diagonaValueControl][column + diagonaValueControl] != null &&
                        currentTable.GetPiecesPosition()[row + diagonaValueControl][column + diagonaValueControl].GetIsBlack() != currentTable.GetPiecesPosition()[row][column].GetIsBlack() &&
                        !piecesToEat.Contains(currentTable.GetPiecesPosition()[row + diagonaValueControl][column + diagonaValueControl]))
                    {
                        if (row < currentTable.GetPiecesPosition().Count - (diagonaValueControl + 1) && column < currentTable.GetPiecesPosition().Count - (diagonaValueControl + 1))
                        {
                            if (currentTable.GetPiecesPosition()[row + (diagonaValueControl + 1)][column + (diagonaValueControl + 1)] == null)

                            {
                                positionUpRight = new List<int>();

                                positionUpRight.Add(row + 2);

                                positionUpRight.Add(column + 2);

                                positionUpRight.Add(row + 1);

                                positionUpRight.Add(column + 1);

                                Debug.Log($"able to eat again at +{diagonaValueControl}+{diagonaValueControl}");
                                checkIfHasToEat = true;
                                diagonalWhileControl = false;
                            }
                        }
                        else
                        {
                            diagonalWhileControl = false;
                        }

                    }

                    else
                    {
                        diagonaValueControl++;
                    }
                }
                else
                {
                    diagonalWhileControl = false;
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
        }
        return checkIfHasToEat;
    }

    public List<Piece> GetListOfPiecesAbleToEat()
    {
        if (piecesThatHasToEatInBegginingOfTurn == null)
        {
            piecesThatHasToEatInBegginingOfTurn = CheckForPiecesThatHasToEatInitaly();
        }
        return piecesThatHasToEatInBegginingOfTurn;
    }


    private List<Piece> CheckForPiecesThatHasToEatInitaly()
    {
        List<Piece> newListOfPiecesAbleToEat = new List<Piece>();

        int rowController = 0;
        int collumnController = 0;
        positionsKingMovePossibilities = new Dictionary<int, List<int>>();

        while (rowController < currentTable.GetCurretBoardSpacePositions()[0].Count)
        {
            Debug.Log(currentTable.GetPiecesPosition()[rowController][collumnController] + "  preça PREOEOOOOOOOOOOOOOOOOOOOOOOOOOOSSSSS");
            if ((currentTable.GetPiecesPosition()[rowController][collumnController] != null) && (currentTable.GetPiecesPosition()[rowController][collumnController].GetIsBlack() == isBlackTurn))
            {

                bool canEatUpLeft = false;
                bool canEatUpRight = false;
                bool canEatDownLeft = false;
                bool canEatDownRight = false;

                bool isPieceInContactWihtOpponentUpLeft = PieceInContactUpLeft(rowController, collumnController, false, true);

                if (isPieceInContactWihtOpponentUpLeft == true && contactPositionUpLeft[1] > 0)
                {
                    Debug.Log(contactPositionUpLeft[0]);
                    Debug.Log(contactPositionUpLeft[1]);
                    Debug.Log("AAAAAAAAAAAAAA CONTACCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCT");
                    canEatUpLeft = PieceInContactUpLeft(contactPositionUpLeft[0], contactPositionUpLeft[1], isPieceInContactWihtOpponentUpLeft, true);

                }
                bool isPieceInContactWihtOpponentUpRight = PieceInContactUpRight(rowController, collumnController, false, true);

                if (isPieceInContactWihtOpponentUpRight)
                {
                    canEatUpRight = PieceInContactUpRight(contactPositionUpRight[0], contactPositionUpRight[1], isPieceInContactWihtOpponentUpRight, true);

                }
                bool isPieceInContactWihtOpponentDownLeft = PieceInContactDownLeft(rowController, collumnController, false, true);
                if (isPieceInContactWihtOpponentDownLeft == true && contactPositionDownLeft[1] > 0)
                {
                    canEatDownLeft = PieceInContactDownLeft(contactPositionDownLeft[0], contactPositionDownLeft[1], isPieceInContactWihtOpponentDownLeft, true);

                }
                bool isPieceInContactWihtOpponentDownRight = PieceInContactDownRight(rowController, collumnController, false, true);
                if (isPieceInContactWihtOpponentDownRight)
                {
                    canEatDownRight = PieceInContactDownRight(contacPositionDownRight[0], contacPositionDownRight[1], isPieceInContactWihtOpponentDownRight, true);
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