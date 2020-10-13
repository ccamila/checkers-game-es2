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

    public void PieceConstructor()
    {

        whitePiecesList = new List<Piece>();
        darkPiecesList = new List<Piece>();

        //pieceGameObject = Resources.Load<GameObject>("Piece");
        //piece = pieceGameObject.GetComponent<Piece>();
        TableConstructor tableContructorInstance = TableConstructor.instance();
        board = tableContructorInstance.GetBoard();
        playbleBoard = tableContructorInstance.GetPlaybleArea();
        /*        Debug.Log(playbleBoard.Count + "AAAAAAAAAAAAAAAAAA");*/
        int totalPieces = playbleBoard.Count * playbleBoard[0].Count - (2 * playbleBoard[0].Count);
        int columnValue = 0;
        int placeController = totalPieces;
        int rowValue = 0;
        GameObject pieceGameObject = Resources.Load<GameObject>("Piece");
        List<GameObject> testList = new List<GameObject>();

        List<Piece> auxiliarPieceList = new List<Piece>();
        List<List<Piece>> auxiliarFinalList = new List<List<Piece>>();


        while (placeController > 0)
        {

            if (rowValue < (playbleBoard.Count / 2 - 1))
            {

                GameObject newPiece = Instantiate(pieceGameObject);
                newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                newPiece.transform.position = playbleBoard[rowValue][columnValue].transform.position;
                /*                Debug.Log(rowValue + "rowvle");
                                Debug.Log(columnValue + "colvle");*/
                playbleBoard[rowValue][columnValue].SetPlayable();
                newPiece.GetComponent<Piece>().SetBlackColor(false);
                whitePiecesList.Add(newPiece.GetComponent<Piece>());
                newPiece.GetComponent<MeshRenderer>().material = whiteMaterial;
                //Debug.Log(pieceGameObject.gameObject.name + " adding");
                //board.SetPiecesPosition(rowValue,columnValue, pieceGameObject.GetComponent<Piece>());
                board.SetPiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                //Debug.Log(pieceGameObject.name + "name now");
                //Piece piece = newPiece.GetComponent< newPiece.GetComponent<Piece>()> ();

                auxiliarPieceList.Add(newPiece.GetComponent<Piece>());
                //testList.Add(newPiece);

/*                for (int i = 0; i < auxiliarPieceList.Count; i++)
                {
                    Debug.Log(auxiliarPieceList[i].gameObject.name + "added now dkt");
                }*/


                placeController--;

            }
            if (rowValue > (playbleBoard.Count / 2))
            {

                GameObject newPiece = null;
                newPiece = Instantiate(pieceGameObject);
                newPiece.name = (rowValue.ToString() + " " + columnValue.ToString());
                newPiece.transform.position = playbleBoard[rowValue][columnValue].transform.position;
                playbleBoard[rowValue][columnValue].SetPlayable();
                newPiece.GetComponent<Piece>().SetBlackColor(true);
                darkPiecesList.Add(newPiece.GetComponent<Piece>());
                newPiece.GetComponent<MeshRenderer>().material = grayMaterial;
                //Debug.Log(pieceGameObject.gameObject.name + " adding");
                //board.SetPiecesPosition(rowValue, columnValue, pieceGameObject.GetComponent<Piece>());
                board.SetPiecesPositionList(rowValue, columnValue, newPiece.GetComponent<Piece>());
                //testList.Add(newPiece);
                //Piece piece = newPiece.GetComponent<Piece>();
                auxiliarPieceList.Add(newPiece.GetComponent<Piece>());
/*                for (int i = 0; i < auxiliarPieceList.Count; i++)
                {
                    Debug.Log(auxiliarPieceList[i].gameObject.name + "added now dkt dsdssd");
                }*/

                placeController--;

            }

            if (columnValue < playbleBoard[0].Count)
            {
                columnValue++;
            }

            if (placeController % playbleBoard[0].Count == 0)
            {
                if (rowValue < playbleBoard.Count)
                {
                    rowValue++;
                }
                columnValue = 0;
                auxiliarFinalList.Add(auxiliarPieceList);
                /*                for (int i = 0; i < auxiliarPieceList.Count; i++)
                                {
                                    Debug.Log(auxiliarPieceList[i].gameObject.name + "current problem");
                                }*/

                // auxiliarPieceList = new List<Piece>();
            }
        }

        /*        for (int i = 0; i < auxiliarFinalList.Count; i++)
                {
                    for (int j = 0; j < auxiliarFinalList[0].Count; j++)
                    {

                        Debug.Log(auxiliarFinalList[i][j].gameObject.name + " " + i + " " + j + "Test correct");
                    }


                }*/
        //board.GetPiecesPositionList();

/*        for (int i = 0; i < auxiliarPieceList.Count; i++)

        {
            Debug.Log(auxiliarPieceList[i].gameObject.name + " test lsit here");
        }
        sendPicesList(auxiliarPieceList);*/
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
            Debug.Log("Sending " + (listToSend[i].gameObject.name + "at row" + rowControl, "and colummun" + columnControl));
            board.SetPiecesPositionList(rowControl, columnControl, listToSend[i]);

        }
    }

}
