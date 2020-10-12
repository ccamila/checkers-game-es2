using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesContructor : MonoBehaviour
{

    [SerializeField]
    private Material grayMaterial;

    [SerializeField]
    private Material whiteMaterial;

    private static PiecesContructor _instance;

    List<List<BoardPiece>> playbleBoard;

    List<Piece> whitePiecesList;
    List<Piece> DarkPiecesList;

    public static PiecesContructor instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindObjectOfType<PiecesContructor>();

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("PiecesContructor");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<PiecesContructor>();
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
        DarkPiecesList = new List<Piece>();

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
                whitePiecesList.Add(pieceGameObject.GetComponent<Piece>());
                pieceGameObject.GetComponent<MeshRenderer>().material = whiteMaterial;
                placeController--;

            }
            else if (rowValue > (playbleBoard.Count / 2))
            {
                GameObject pieceGameObject = Resources.Load<GameObject>("Piece");
                Instantiate(pieceGameObject);
                pieceGameObject.transform.position = playbleBoard[rowValue][columnValue].transform.position;
                placeController--;
                pieceGameObject.GetComponent<MeshRenderer>().material = grayMaterial;
                DarkPiecesList.Add(pieceGameObject.GetComponent<Piece>());

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
}
