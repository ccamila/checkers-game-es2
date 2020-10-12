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
        playbleBoard = tableContructorInstance.GetPlaybleArea();
        int totalPieces = playbleBoard.Count * playbleBoard[0].Count - (2 * playbleBoard[0].Count);
        int columnValue = 0;
        int placeController = totalPieces;
        int rowValue = 0;

        while (placeController > 0)
        {

            if (rowValue < (playbleBoard.Count / 2 - 1))
            {
                GameObject pieceGameObject = Resources.Load<GameObject>("Piece");
                Instantiate(pieceGameObject);
                pieceGameObject.transform.position = playbleBoard[rowValue][columnValue].transform.position;
                playbleBoard[rowValue][columnValue].SetPlayable();
                pieceGameObject.GetComponent<Piece>().SetBlackColor(false);
                whitePiecesList.Add(pieceGameObject.GetComponent<Piece>());
                pieceGameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
                placeController--;

            }
            else if (rowValue > (playbleBoard.Count / 2))
            {
                GameObject pieceGameObject = Resources.Load<GameObject>("Piece");
                Instantiate(pieceGameObject);
                pieceGameObject.transform.position = playbleBoard[rowValue][columnValue].transform.position;
                playbleBoard[rowValue][columnValue].SetPlayable();
                pieceGameObject.GetComponent<MeshRenderer>().material = grayMaterial;
                pieceGameObject.GetComponent<Piece>().SetBlackColor(true);
                darkPiecesList.Add(pieceGameObject.GetComponent<Piece>());
                placeController--;

            }

            if (columnValue < playbleBoard[0].Count )
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
}
