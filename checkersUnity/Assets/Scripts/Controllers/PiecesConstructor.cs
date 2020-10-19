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

        whitePiecesList = new List<Piece>();
        darkPiecesList = new List<Piece>();

        TableConstructor tableContructorInstance = TableConstructor.instance();
        board = tableContructorInstance.GetBoard();
        playbleBoard = tableContructorInstance.GetPlaybleArea();

        int totalPieces = playbleBoard.Count * playbleBoard[0].Count - (2 * playbleBoard[0].Count);
        int columnValue = 0;
        int placeController = totalPieces;
        int rowValue = 0;
        GameObject pieceGameObject = Resources.Load<GameObject>("Piece");

        while (placeController > 0)
        {
            GameObject newPiece = Instantiate(pieceGameObject);


            if (rowValue < (tableContructorInstance.GetPlaybleArea().Count / 2 - 1))
            {
                newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                newPiece.transform.position = tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].transform.position;
                tableContructorInstance.SetPlaybleArea(rowValue,columnValue);
                newPiece.GetComponent<Piece>().SetBlackColor(false);
                whitePiecesList.Add(newPiece.GetComponent<Piece>());
                newPiece.GetComponent<MeshRenderer>().material = whiteMaterial;
                board.SetPiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                placeController--;
            }
            else if (rowValue > (tableContructorInstance.GetPlaybleArea().Count / 2))
            {
                newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                newPiece.transform.position = tableContructorInstance.GetPlaybleArea()[rowValue][columnValue].transform.position;
                tableContructorInstance.SetPlaybleArea(rowValue, columnValue);
                newPiece.GetComponent<Piece>().SetBlackColor(true);
                darkPiecesList.Add(newPiece.GetComponent<Piece>());
                newPiece.GetComponent<MeshRenderer>().material = grayMaterial;
                board.SetPiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                placeController--;
            }

            if (columnValue < tableContructorInstance.GetPlaybleArea()[0].Count)
            {
                columnValue++;
            }

            if (placeController % tableContructorInstance.GetPlaybleArea()[0].Count == 0)
            {
                if (rowValue < tableContructorInstance.GetPlaybleArea().Count)
                {
                    rowValue++;
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
            board.SetPiecesPositionList(rowControl, columnControl, listToSend[i]);

        }
    }

}
