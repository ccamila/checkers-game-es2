using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesConstructor : MonoBehaviour
{

    [SerializeField]
    private Material grayMaterial;

    [SerializeField]
    private Material whiteMaterial;

    private static PiecesConstructor _instance;

    List<List<BoardPiece>> playbleBoard;

    List<Piece> whitePiecesList;
    List<Piece> darkPiecesList;

    Board board;

    List<List<Piece>> piecesPosition;

    public static PiecesConstructor instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindObjectOfType<PiecesConstructor>();

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("PiecesConstructor");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<PiecesConstructor>();
                DontDestroyOnLoad(instanceObject);
            }
            else
                Debug.Log("Resource does not have a definition for PiecesContructor");
        }
        return _instance;
    }

    public void ConstructPieces()
    {
        piecesPosition = new List<List<Piece>>();
        whitePiecesList = new List<Piece>();
        darkPiecesList = new List<Piece>();

        TableConstructor tableContructorInstance = TableConstructor.instance();
        board = tableContructorInstance.GetBoard();
        playbleBoard = tableContructorInstance.GetPlaybleArea();

        int totalPieces = board.GetSizeOfTable() * 3;
        int columnValue = 0;
        int placeController = totalPieces;
        int rowValue = 0;
        List<Piece> auxiliarPiecesList = StartEmptyList();
        GameObject pieceGameObject = Resources.Load<GameObject>("Piece");

        while (placeController > 0)
        {

            if (tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].IsPlayable())
            {

                if (rowValue < (tableContructorInstance.GetPlaybleArea().Count / 2 - 1))
                {
                    GameObject newPiece = Instantiate(pieceGameObject);
                    newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                    newPiece.transform.position = tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].transform.position;
                    tableContructorInstance.SetPlaybleTile(rowValue, columnValue);
                    newPiece.GetComponent<Piece>().SetIsKing(false);
                    newPiece.GetComponent<Piece>().SetIsUp(false);
                    newPiece.GetComponent<Piece>().SetBlackColor(false);
                    newPiece.GetComponent<Piece>().SetIsAvaiableToEat(true);
                    whitePiecesList.Add(newPiece.GetComponent<Piece>());
                    newPiece.GetComponent<MeshRenderer>().material = whiteMaterial;
                    board.UpdatePiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                    placeController--;
                    auxiliarPiecesList[columnValue] = newPiece.GetComponent<Piece>();
                }
                else if (rowValue > (tableContructorInstance.GetPlaybleArea().Count / 2))
                {
                    GameObject newPiece = Instantiate(pieceGameObject);
                    newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                    newPiece.transform.position = tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].transform.position;
                    newPiece.GetComponent<Piece>().SetIsKing(false);
                    newPiece.GetComponent<Piece>().SetIsUp(true);
                    tableContructorInstance.SetPlaybleTile(rowValue, columnValue);
                    newPiece.GetComponent<Piece>().SetBlackColor(true);
                    newPiece.GetComponent<Piece>().SetIsAvaiableToEat(true);
                    darkPiecesList.Add(newPiece.GetComponent<Piece>());
                    newPiece.GetComponent<MeshRenderer>().material = grayMaterial;
                    board.UpdatePiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                    placeController--;
                    auxiliarPiecesList[columnValue] = newPiece.GetComponent<Piece>();
                }
            }

            if (columnValue < tableContructorInstance.GetPlaybleArea()[0].Count - 1)
            {
                columnValue++;
            }

            if (placeController % (tableContructorInstance.GetPlaybleArea()[0].Count / 2) == 0 && (!tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].IsPlayable()))
            {
                if (rowValue < tableContructorInstance.GetPlaybleArea().Count)
                {
                    rowValue++;
                    piecesPosition.Add(auxiliarPiecesList);
                    auxiliarPiecesList = StartEmptyList();
                }
                columnValue = 0;
            }
        }
    }

    public List<Piece> getWhitePiece()
    {
        return whitePiecesList;
    }
    public List<Piece> getDarkPiece()
    {
        return darkPiecesList;
    }
    public void sendPicesList(List<Piece> listToSend)
    {
        int rowControl = -1;
        int columnControl = 0;
        for (int i = 0; i < listToSend.Count; i++)
        {

            if (i % 4 == 0)
            {
                rowControl++;
                columnControl = 0;
            }
            else
            {
                columnControl++;
            }
            /*            Debug.Log("Sending " + (listToSend[i].gameObject.name + "at row" + rowControl, "and colummun" + columnControl));*/
            board.UpdatePiecesPositionList(rowControl, columnControl, listToSend[i]);

        }
    }
    private List<Piece> StartEmptyList()
    {
        List<Piece> emptyList = new List<Piece>();
        for (int i = 0; i < board.GetSizeOfTable(); i++)
        {
            Piece nullPiece = null;
            emptyList.Add(nullPiece);
        }
        return emptyList;

    }
    public List<List<Piece>> GetPiecesPosition()
    {
        return piecesPosition;
    }

}
