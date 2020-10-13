using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    List<List<BoardPiece>> playbleBoard;
    List<List<Piece>> checkersPiecesPositions;
    private bool turnController = false; // true black, false white 
    Piece pieceToUpdate;
    int[] currentPos;
    int[] newPos;


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

    public void SetPiece(Piece piece)
    {
        pieceToUpdate = piece;
    }

    public void SetNewPOs(int[] pos)
    {
        newPos = pos;
    }

    public void SetOldPOs(int[] pos)
    {
        currentPos = pos;
    }
    public void updateGameobject() 
    {
        Vector2 newpos = new Vector2(newPos[0], newPos[1]);
        Debug.Log(newpos[0] + " " + newPos[1]);
        Debug.Log(pieceToUpdate);
        pieceToUpdate.gameObject.gameObject.transform.position = newpos;
        
    }
    private void Awake()
    {
        tableConstructor = TableConstructor.instance();
        piecesConstructor = PiecesConstructor.instance();
        //playbleBoard = tableConstructor.GetPlaybleArea();
        //checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();
    }

/*    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            checkersPiecesPositions = tableConstructor.GetBoard().GetPiecesPositionList();

*//*            for (int i = 0; i < checkersPiecesPositions.Count; i++)
            {
                for (int j = 0; j < checkersPiecesPositions[0].Count; j++)
                {
                    Debug.Log(checkersPiecesPositions[i][j].gameObject.name + " " + i + " " + j);
                }

                //Debug.Log(checkersPiecesPositions[i].Contains(auxiliarPiece));
            }*//*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log(hit.transform.gameObject.name);
                Piece auxiliarPiece = hit.transform.gameObject.GetComponent<Piece>();

                //Debug.Log(System.Array.IndexOf(checkersPiecesPositions,auxiliarPiece));

                //int row = checkersPiecesPositions.GetLength(0);
                //int column
*//*                for (int i = 0; i < checkersPiecesPositions.Count; i++)
                {
                    for (int j = 0; j < checkersPiecesPositions[0].Count; j++)
                    {
                        Debug.Log(checkersPiecesPositions[i][j].gameObject.name + " " + i + " " + j);
                    }

                    //Debug.Log(checkersPiecesPositions[i].Contains(auxiliarPiece));
                }*//*
                //Debug.Log(checkersPiecesPositions.Find(hit.transform.gameObject.GetComponent<Piece>()));
                //playbleBoard[0].Find(hit.transform.gameObject.GetComponent<BoardPiece>());
            }
        }
    }*/

}
